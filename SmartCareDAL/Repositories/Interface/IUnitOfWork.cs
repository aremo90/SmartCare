using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Interface
{
    public interface IUnitOfWork 
    {
        Task<int> SaveChangesAsync();
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();


    }
}
