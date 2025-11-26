using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.DeviceDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeviceService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<DeviceDTO> GetDeviceInfoByUserId(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }
            var deviceRepository = _unitOfWork.GetRepository<Device, int>();

            // This approach fetches all devices and filters in memory.
            // For better performance, consider enhancing IGenericRepository to support server-side filtering.
            var device = (await deviceRepository.GetAllAsync()).FirstOrDefault(d => d.UserId == userId);

            if (device == null)
            {
                return null;
            }

            return  _mapper.Map<DeviceDTO>(device);

        }

        public async Task<DeviceDTO> RegisterDeviceForUser(string userId, CreateDeviceDTO createDeviceDTO)
        {
            if (userId is null)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }

            if (createDeviceDTO == null)
            {
                throw new ArgumentNullException(nameof(createDeviceDTO));
            }

            if (string.IsNullOrWhiteSpace(createDeviceDTO.DeviceIdentifier))
            {
                throw new ArgumentException("Device identifier cannot be empty.", nameof(createDeviceDTO.DeviceIdentifier));
            }

            var deviceRepository = _unitOfWork.GetRepository<Device, int>();
            var allDevices = await deviceRepository.GetAllAsync();

            if (allDevices.Any(d => d.UserId == userId))
            {
                throw new InvalidOperationException("A device is already registered for this user.");
            }

            if (allDevices.Any(d => d.DeviceIdentifier == createDeviceDTO.DeviceIdentifier))
            {
                throw new InvalidOperationException("This device identifier is already in use.");
            }

            var newDevice = _mapper.Map<Device>(createDeviceDTO);
            newDevice.UserId = userId;

            await deviceRepository.AddAsync(newDevice);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DeviceDTO>(newDevice);
        }
    }
}
