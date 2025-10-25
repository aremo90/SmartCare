using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ✅ Get all users
        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        // ✅ Get user by ID with better structure
        public async Task<UserViewModel?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid user ID provided.", nameof(id));

            var user = await _unitOfWork.Users.GetByIdAsync(id).ConfigureAwait(false);
            return user is null ? null : _mapper.Map<UserViewModel>(user);
        }

        // ✅ Get user by email
        public async Task<UserViewModel?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            var user = (await _unitOfWork.Users.FindAsync(u => u.Email == email)
                .ConfigureAwait(false))
                .FirstOrDefault();

            return user is null ? null : _mapper.Map<UserViewModel>(user);
        }

        // ✅ Update user with better response
        public async Task<UserViewModel?> UpdateAsync(int id, UserUpdateViewModel model)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                var existing = (await _unitOfWork.Users.FindAsync(u => u.Email == model.Email)).FirstOrDefault();
                if (existing != null)
                    throw new ApplicationException($"Email '{model.Email}' is already taken.");
                user.Email = model.Email;
            }

            if (!string.IsNullOrEmpty(model.FirstName))
                user.FirstName = model.FirstName;

            if (!string.IsNullOrEmpty(model.LastName))
                user.LastName = model.LastName;

            user.UpdatedAt = DateTime.UtcNow;

            // ✅ Explicitly mark entity as updated
            _unitOfWork.Users.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserViewModel>(user);
        }


        // ✅ Delete user safely
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id).ConfigureAwait(false);
            if (user is null)
                return false;

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }

    }
}
