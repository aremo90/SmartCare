using LinkO.Shared.DTOS.DeviceDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IDeviceService
    {
        Task<DeviceDTO> GetDeviceInfoByUserId(string userId);
        Task<DeviceDTO> RegisterDeviceForUser(string userId, CreateDeviceDTO createDeviceDTO);
    }
}
