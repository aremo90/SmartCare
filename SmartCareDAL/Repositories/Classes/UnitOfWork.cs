using Microsoft.EntityFrameworkCore;
using SmartCareDAL.Data.Context;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SmartCareDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories; // ✅ initialized in constructor

        public IUserRepository Users { get; }
        public IMedicineReminderRepository MedicineReminders { get; }
        public IDeviceCommandRepository DeviceCommands { get; }
        public IDeviceRepository Devices { get; }

        public UnitOfWork(
            SmartCareDbContext dbContext,
            IUserRepository userRepository,
            IMedicineReminderRepository medicineReminderRepository,
            IDeviceCommandRepository deviceCommandRepo,
            IDeviceRepository deviceRepository)
        {
            _dbContext = dbContext;
            Users = userRepository;
            MedicineReminders = medicineReminderRepository;
            DeviceCommands = deviceCommandRepo;
            Devices = deviceRepository;

            _repositories = new Dictionary<Type, object>(); // ✅ Initialize dictionary
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new GenericRepository<TEntity>(_dbContext);
                _repositories[type] = repoInstance;
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
