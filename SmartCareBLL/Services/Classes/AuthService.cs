using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartCareBLL.DTOS.AuthDTOS;
using SmartCareBLL.Services.Interfaces;
using SmartCareDAL.Models;
using SmartCareDAL.Models.Enum;
using SmartCareDAL.Repositories.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SmartCareBLL.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration,IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _userRepository = userRepository;
        }


        // Login
        public async Task<(string Token, int UserID)> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            

            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new ApplicationException("Invalid email or password");


            var token = GenerateToken(user);
            return (token, user.Id);
        }

        // Signup
        public async Task<User> RegisterAsync(string firstName, string lastName, string email, string password, string gender, DateTime dateOfBirth, string phoneNumber)
        {
            // 1️ Check if email already exists
            var exists = await _userRepository.GetByEmailAsync(email);
            if (exists != null)
                throw new ApplicationException($"Email '{email}' is already taken.");

            // 2 Create new user entity
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = HashPassword(password),
                Gender = Enum.Parse<Gender>(gender, ignoreCase: true),
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber.Trim(),
                CreatedAt = DateTime.Now
            };

            // 3 Save changes to database
            await _unitOfWork.GetRepository<User>().AddAsync(user);
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
