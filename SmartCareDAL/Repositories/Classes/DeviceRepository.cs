using SmartCareDAL.Data.Context;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SmartCareDAL.Repositories.Classes
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        private readonly SmartCareDbContext _context;

        public DeviceRepository(SmartCareDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Device?> GetByUserIdAsync(int userId)
        {
            return await _context.Devices.FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<Device?> GetByIdentifierAsync(string deviceIdentifier)
        {
            return await _context.Devices.FirstOrDefaultAsync(d => d.DeviceIdentifier == deviceIdentifier);
        }
    }
}
