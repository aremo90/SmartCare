using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using LinkO.ServiceAbstraction;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.DeviceDTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeviceService(IUnitOfWork unitOfWork , IMapper mapper , UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<Result<DeviceDTO>> GetDeviceInfoByUserId(string Email)
        {
            var User = _userManager.FindByEmailAsync(Email).Result;
            if (User is null)
                return Error.NotFound("User Not found");

            var deviceRepository = _unitOfWork.GetRepository<Device, int>();
            var device = (await deviceRepository.GetAllAsync()).FirstOrDefault(d => d.UserId == User.Id);

            if (device is null)
                return Error.NotFound("Device not Found");

            return  _mapper.Map<DeviceDTO>(device);
        }

        public async Task<Result<DeviceDTO>> RegisterDeviceForUser(string Email, CreateDeviceDTO createDeviceDTO)
        {
            var User = _userManager.FindByEmailAsync(Email).Result;
            if (User is null)
                return Error.NotFound("User Not found");

            if (createDeviceDTO == null)
                return Error.InvalidCredentials();

            if (string.IsNullOrWhiteSpace(createDeviceDTO.DeviceIdentifier))
                return Error.InvalidCredentials("Device identifier cannot be empty.");

            var deviceRepository = _unitOfWork.GetRepository<Device, int>();
            var allDevices = await deviceRepository.GetAllAsync();

            if (allDevices.Any(d => d.UserId == User.Id))
                return Error.InvalidCredentials("User Already Have A Device Paired");

            if (allDevices.Any(d => d.DeviceIdentifier == createDeviceDTO.DeviceIdentifier))
                return Error.InvalidCredentials("This Device is Already Paired");

            var newDevice = _mapper.Map<Device>(createDeviceDTO);
            newDevice.UserId = User.Id;

            await deviceRepository.AddAsync(newDevice);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DeviceDTO>(newDevice);
        }



    }
}
