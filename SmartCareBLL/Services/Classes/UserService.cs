using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartCareBLL.DTOS.UserDTOS;
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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var Users = await _unitOfWork.GetRepository<User>().GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(Users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> DeleteUserByIdAsync(int id)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }


        public async Task<UserDTO> UpdateUserByIdAsync(int id, UserToUpdateDTO userDto)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(userDto.FirstName))
            {
                user.FirstName = userDto.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(userDto.LastName))
            {
                user.LastName = userDto.LastName;
            }

            if (!string.IsNullOrWhiteSpace(userDto.PhoneNumber))
            {
                user.PhoneNumber = userDto.PhoneNumber;
            }

            _unitOfWork.GetRepository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }
    }
}
