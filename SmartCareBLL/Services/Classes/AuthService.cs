using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;
using SmartCareBLL.ViewModels.LoginViewModel;
using SmartCareDAL.Data.Context;
using SmartCareDAL.Models;
using SmartCareDAL.Models.Enum;
using SmartCareDAL.Repositories.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace SmartCareBLL.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // Login
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Email == email)).FirstOrDefault();
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new ApplicationException("Invalid email or password");

            return GenerateToken(user);
        }

        // Signup
        public async Task<User> RegisterAsync(string firstName, string lastName, string email, string password, string gender, DateTime dateOfBirth , string phoneNumber)
        {
            var exists = (await _unitOfWork.Users.FindAsync(u => u.Email == email)).Any();
            if (exists) throw new ApplicationException($"Email '{email}' is already taken.");

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = HashPassword(password),
                Gender = Enum.Parse<Gender>(gender),
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        // JWT generation
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("FullName", $"{user.FirstName} {user.LastName}")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpiresInHours"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #region Helper
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }
        #endregion
    }
}
