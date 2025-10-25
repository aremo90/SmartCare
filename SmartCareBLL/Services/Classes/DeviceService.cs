using Microsoft.EntityFrameworkCore;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.DeviceViewModel;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Classes
{
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeviceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 🔹 Register a new device for a user
        public async Task<Device?> RegisterDeviceAsync(int userId, string deviceIdentifier, string? model)
        {
            // Check if user exists
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Check if this device already exists
            var existingDevice = await _unitOfWork.GetRepository<Device>()
                .FirstOrDefaultAsync(d => d.DeviceIdentifier == deviceIdentifier);

            if (existingDevice != null)
                return null;
                //throw new Exception("Device already registered.");

            var newDevice = new Device
            {
                UserId = userId,
                DeviceIdentifier = deviceIdentifier,
                Model = model,
                IsActive = true,
                IsPaired = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Devices.AddAsync(newDevice);
            await _unitOfWork.SaveChangesAsync();

            return newDevice;
        }

        // 🔹 Pair an existing device to a user
        public async Task<bool> PairDeviceAsync(int userId, string deviceIdentifier)
        {
            var device = await _unitOfWork.Devices
                .FirstOrDefaultAsync(d => d.DeviceIdentifier == deviceIdentifier);

            if (device == null)
                return false;

            device.UserId = userId;
            device.IsPaired = true;
            device.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Devices.Update(device);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // 🔹 Update status info (online/offline, signal, etc.)
        public async Task<bool> UpdateStatusAsync(string deviceIdentifier, bool isActive, double? signalStrength)
        {
            var device = await _unitOfWork.Devices
                .FirstOrDefaultAsync(d => d.DeviceIdentifier == deviceIdentifier);

            if (device == null)
                return false;

            device.IsActive = isActive;
            device.SignalStrength = signalStrength;
            device.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Devices.Update(device);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // 🔹 Get device by user ID
        public async Task<DeviceViewModel?> GetDeviceByUserIdAsync(int userId)
        {
            var device = await _unitOfWork
                .Devices
                .FindAsync(d => d.UserId == userId);

            var deviceEntity = device.FirstOrDefault();
            if (deviceEntity == null)
                return null;

            return new DeviceViewModel
            {
                Id = deviceEntity.Id,
                DeviceIdentifier = deviceEntity.DeviceIdentifier,
                Model = deviceEntity.Model,
                IsActive = deviceEntity.IsActive,
                SignalStrength = deviceEntity.SignalStrength,
                UserId = deviceEntity.UserId,
                CreatedAt = deviceEntity.CreatedAt,
                UpdatedAt = DateTime.Now
            };
        }
    }
}
