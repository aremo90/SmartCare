using Microsoft.AspNetCore.Authentication.JwtBearer;
using LinkO.Domin.Models.IdentityModule;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AuthDTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using LinkO.Domin.Models.Enum;
using LinkO.Shared.CommonResult;

namespace LinkO.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager , IConfiguration configuration)
        {
           _userManager = userManager;
           _configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User != null;
        }

        public async Task<Result<UserInfoDTO>> GetUserByEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                return Error.NotFound("User Info Not Found");
            return new UserInfoDTO(User.FirstName , User.LastName , User.Email! , User.Gender.ToString() , User.DateOfBirth , User.PhoneNumber!);
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
                return Error.InvalidCredentials("Invalid Email or Password");
            var isPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (!isPasswordValid)
                return Error.InvalidCredentials("Invalid Email or Password");
      
            var Token = await GenrateTokenAsync(User);
            return new UserDTO(User.Email! , User.FirstName, Token);

        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                Gender = Enum.Parse<Gender>(registerDTO.Gender, true),
                DateOfBirth = registerDTO.DateOfBirth,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = $"{registerDTO.FirstName}{registerDTO.LastName}{Guid.NewGuid().ToString("N")[..6]}"
            };
            var IdentityResult  =  await _userManager.CreateAsync(User, registerDTO.Password);
            if (IdentityResult.Succeeded)
            {
                var Token = await GenrateTokenAsync(User);
                return new UserDTO(User.Email!, User.FirstName, Token);
            }
            return IdentityResult.Errors.Select(E => Error.Validation(E.Code , E.Description)).ToList();

        }
        public async Task<Result<UserInfoDTO>> UpdateUserProfile(string email, UpdateUserInfo updateUserInfo)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                return Error.NotFound();

            if (updateUserInfo is null)
                return Error.InvalidCredentials();

            if (updateUserInfo.PhoneNumber is not null)
                User.PhoneNumber = updateUserInfo.PhoneNumber;
            if (updateUserInfo.FirstName is not null)
                User.FirstName = updateUserInfo.FirstName;
            if (updateUserInfo.LastName is not null)
                User.LastName = updateUserInfo.LastName;
            if (updateUserInfo.DateOfBirth is not null)
                User.DateOfBirth = (DateOnly)updateUserInfo.DateOfBirth;
            if (updateUserInfo.PublicId is not null )
                User.PublicId = updateUserInfo.PublicId;
            if (updateUserInfo.ProfilePicture is not null)
                User.ProfilePicture = updateUserInfo.ProfilePicture;

            await _userManager.UpdateAsync(User);
            return new UserInfoDTO(User.FirstName, User.LastName, User.Email!, User.Gender.ToString(), User.DateOfBirth, User.PhoneNumber!, User.PublicId, User.ProfilePicture);
        }



        #region JWT Token

        private async Task<string> GenrateTokenAsync(ApplicationUser User) 
        {
            // Claims

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, User.FirstName!),
                new Claim(JwtRegisteredClaimNames.Email, User.Email!),
            };

            // Roles [Admin]

            var Roles = await _userManager.GetRolesAsync(User);
            foreach (var role in Roles)
            {
                claims.Add(new Claim("roles", role));
            }


            // Secret Key
            var secretKey = _configuration["JWTOptions:secretKey"]; 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            // Signing Credentials
            var Cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create Token
            var Token = new JwtSecurityToken(
                issuer      : _configuration["JWTOptions:Issuer"],
                audience    : _configuration["JWTOptions:Audience"],
                expires     : DateTime.Now.AddHours(2),
                claims      : claims,
                signingCredentials : Cred
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        #endregion
    }
}
