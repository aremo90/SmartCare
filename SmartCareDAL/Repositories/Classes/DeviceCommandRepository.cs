using SmartCareDAL.Data;
using SmartCareDAL.Data.Context;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;
using System;

namespace SmartCareDAL.Repositories.Classes
{
    public class DeviceCommandRepository : GenericRepository<DeviceCommand>, IDeviceCommandRepository
    {
        public DeviceCommandRepository(SmartCareDbContext context) : base(context)
        {
        }
    }
}
