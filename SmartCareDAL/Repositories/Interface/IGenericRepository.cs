using SmartCareDAL.Models;
using SmartCareDAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
