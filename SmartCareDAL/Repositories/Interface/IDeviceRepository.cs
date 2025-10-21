using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Interface
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        Task<Device?> GetByUserIdAsync(int userId);
        Task<Device?> GetByIdentifierAsync(string deviceIdentifier);
    }
}
