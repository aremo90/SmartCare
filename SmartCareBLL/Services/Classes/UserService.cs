using AutoMapper;
using AutoMapper.Execution;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;
using SmartCareDAL.Models;
using SmartCareDAL.Models.Enum;
using SmartCareDAL.Repositories.Classes;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        public async Task<UserViewModel?> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserViewModel>(user);
        }

        public async Task<UserViewModel?> GetByEmailAsync(string email)
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.Email == email);
            var user = users.FirstOrDefault();
            return user == null ? null : _mapper.Map<UserViewModel>(user);
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateViewModel model)
        {
            // 1. Get the existing user
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return false; // User not found

            // 2. Check for duplicate email if email is being updated
            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                var existing = (await _unitOfWork.Users.FindAsync(u => u.Email == model.Email)).FirstOrDefault();
                if (existing != null)
                    throw new ApplicationException($"Email '{model.Email}' is already taken.");

                user.Email = model.Email; // update email
            }

            // 3. Update first and last name if provided
            if (!string.IsNullOrEmpty(model.FirstName))
                user.FirstName = model.FirstName;

            if (!string.IsNullOrEmpty(model.LastName))
                user.LastName = model.LastName;

            // 4. Update timestamp
            user.UpdatedAt = DateTime.UtcNow;

            // 5. Save changes
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return false;

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
      
    }
}
