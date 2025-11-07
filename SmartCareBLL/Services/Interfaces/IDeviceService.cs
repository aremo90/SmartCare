using SmartCareBLL.DTOS.DeviceDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<DeviceDTO> GetDeviceInfoByUserId(int userId);
        Task<DeviceDTO> RegisterDeviceForUser(int userId, CreateDeviceDTO createDeviceDTO);
    }
}
