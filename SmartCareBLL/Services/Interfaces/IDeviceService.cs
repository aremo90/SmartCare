using SmartCareBLL.ViewModels.DeviceViewModel;
using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<Device?> RegisterDeviceAsync(int userId, string deviceIdentifier, string? model);
        Task<bool> PairDeviceAsync(int userId, string deviceIdentifier);
        Task<bool> UpdateStatusAsync(string deviceIdentifier, bool isActive, double? signalStrength);
        Task<DeviceViewModel?> GetDeviceByUserIdAsync(int userId);
    }
}
