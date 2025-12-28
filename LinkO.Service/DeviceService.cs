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

        public async Task<Result<DeviceDTO>> AddDeviceAsync(string DeviceIdentifier)
        {
            if (string.IsNullOrWhiteSpace(DeviceIdentifier))
                return Error.InvalidCredentials("Device identifier cannot be empty.");
            var deviceRepository = _unitOfWork.GetRepository<Device, int>();
            var allDevices = await deviceRepository.GetAllAsync();
            if (allDevices.Any(d => d.DeviceIdentifier == DeviceIdentifier))
                return Error.Conflict("Device with the same identifier already exists.");
            var newDevice = new Device { DeviceIdentifier = DeviceIdentifier };
            await deviceRepository.AddAsync(newDevice);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<DeviceDTO>(newDevice);
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
            var User = await _userManager.FindByEmailAsync(Email);
            if (User is null)
                return Error.NotFound("User Not found");

            if (createDeviceDTO == null)
                return Error.InvalidCredentials();

            if (string.IsNullOrWhiteSpace(createDeviceDTO.DeviceIdentifier))
                return Error.InvalidCredentials("Device identifier cannot be empty.");

            var deviceRepository = _unitOfWork.GetRepository<Device, int>();

            // Ideally, filter this in the DB, not in memory (see note below)
            var allDevices = await deviceRepository.GetAllAsync();

            if (allDevices.Any(d => d.UserId == User.Id))
                return Error.InvalidCredentials("User Already Have A Device Paired");

            // 1. FIND the specific existing device object
            var existingDevice = allDevices.FirstOrDefault(d => d.DeviceIdentifier == createDeviceDTO.DeviceIdentifier);

            // Check if it exists
            if (existingDevice == null)
                return Error.NotFound("Device Not found in our records");

            // Check if it's already paired
            if (existingDevice.UserId is not null)
                return Error.InvalidCredentials("This Device is Already Paired");

            // 2. UPDATE the EXISTING object instead of creating a new one
            existingDevice.UserId = User.Id;

            // If createDeviceDTO has other updates (like name), map them onto the existing entity:
             _mapper.Map(createDeviceDTO, existingDevice);

            // 3. Save the existing entity
            deviceRepository.Update(existingDevice);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DeviceDTO>(existingDevice);
        }



    }
}
