using SmartCareBLL.DTOS.GpsDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IGpsService
    {
        Task UpdateGpsLocationAsync(string deviceIdentifier, GpsUpdateDTO gpsUpdateDTO);
        Task<GpsDTO> GetGpsLocationAsync(int userId);
    }
}
