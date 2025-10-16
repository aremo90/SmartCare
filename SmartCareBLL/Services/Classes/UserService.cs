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

        public async Task<UserViewModel> CreateAsync(UserCreateViewModel model)
        {
            var entity = _mapper.Map<User>(model);
            await _unitOfWork.Users.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserViewModel>(entity);
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateViewModel model)
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null) return false;

            _mapper.Map(model, existingUser);
            _unitOfWork.Users.Update(existingUser);
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
