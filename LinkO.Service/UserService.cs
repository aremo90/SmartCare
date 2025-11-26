using AutoMapper;
using Linko.Service.Specification.Users_Specifictaion;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.UserDTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkO.Services
{
    public class UserService : IUserService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper , UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {

            //var Spec = new UserWithDeviceInfoSpecification();

            //var Users = await _unitOfWork.GetRepository<User,string>().GetAllAsync();
            //return _mapper.Map<IEnumerable<UserDTO>>(Users);
            throw new Exception();
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            //var Spec = new UserWithDeviceInfoSpecification(userId);
            //var user = await _unitOfWork.GetRepository<User, string>().GetByIdAsync(userId);
            var User = await _userManager.FindByIdAsync(userId);
            return new UserDTO
            {
                Id = User!.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                PhoneNumber = User.PhoneNumber,
                DateOfBirth = User.DateOfBirth,
            };
                //return _mapper.Map<UserDTO>(User);
            }

        public async Task<UserDTO> DeleteUserByIdAsync(string id)
        {
            //var userRepository = _unitOfWork.GetRepository<User, string>();
            //var user = await userRepository.GetByIdAsync(id);
            //if (user == null)
            //{
            //    return null;
            //}

            //userRepository.Delete(user);
            //await _unitOfWork.SaveChangesAsync();

            //return _mapper.Map<UserDTO>(user);
            throw new Exception();

        }


        public async Task<UserDTO> UpdateUserByIdAsync(string id, UserToUpdateDTO userDto)
        {
            //var user = await 

            //if (user == null)
            //{
            //    return null;
            //}

            //if (!string.IsNullOrWhiteSpace(userDto.FirstName))
            //{
            //    user.FirstName = userDto.FirstName;
            //}

            //if (!string.IsNullOrWhiteSpace(userDto.LastName))
            //{
            //    user.LastName = userDto.LastName;
            //}

            //if (!string.IsNullOrWhiteSpace(userDto.PhoneNumber))
            //{
            //    user.PhoneNumber = userDto.PhoneNumber;
            //}

            //_unitOfWork.GetRepository<User, string>().Update(user);
            //await _unitOfWork.SaveChangesAsync();

            //return _mapper.Map<UserDTO>(user);
            throw new Exception();

        }
    }
}
