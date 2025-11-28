using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.GpsDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IGpsService
    {
        Task UpdateGpsLocationAsync(string deviceIdentifier, GpsUpdateDTO gpsUpdateDTO);
        Task<Result<GpsDTO>> GetGpsLocationAsync(string Email);
    }
}
