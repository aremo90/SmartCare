using LinkO.Shared.CommonResult;
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
        Task<Result<DeviceDTO>> GetDeviceInfoByUserId(string Email);
        Task<Result<DeviceDTO>> RegisterDeviceForUser(string Email, CreateDeviceDTO createDeviceDTO);
        Task<Result<DeviceDTO>> AddDeviceAsync(string DeviceIdentifier);
    }
}
