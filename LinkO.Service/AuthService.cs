using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.Enum;
using LinkO.Domin.Models.IdentityModule;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AuthDTOS;
using LinkO.Shared.DTOS.EnumDTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LinkO.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
           _userManager = userManager;
        }

        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
                throw new Exception("Invalid Email or Password");
            var isPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (!isPasswordValid)
                throw new Exception("Invalid Email or Password");
            return new UserDTO(User.Email! , User.FirstName, "Token");

        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                Gender = Enum.Parse<Gender>(registerDTO.Gender, true),
                DateOfBirth = registerDTO.DateOfBirth,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = "Hamza"
            };
            var IdentityResult  =  await _userManager.CreateAsync(User, registerDTO.Password);
            if (!IdentityResult.Succeeded)
            {
                var errors = string.Join(", ", IdentityResult.Errors.Select(e => e.Description));
                // It's better to throw a more specific exception, like an ArgumentException or a custom one.
                throw new Exception($"User registration failed: {errors}");
            }

            return new UserDTO(User.Email!, User.FirstName, "Token");

        }
    }
}
