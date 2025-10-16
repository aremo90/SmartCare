using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        // Custom Repositories (only ones with domain logic)
        IUserRepository Users { get; }
        IMedicineReminderRepository MedicineReminders { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
