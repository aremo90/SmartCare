using LinkO.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Contract
{
    public interface IUnitOfWork 
    {
        Task<int> SaveChangesAsync();
        IGenericRepository<TEntity, Tkey > GetRepository<TEntity , Tkey>() where TEntity : BaseEntity, new();


    }
}
