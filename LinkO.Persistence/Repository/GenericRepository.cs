using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Persistence.Data;
using LinkO.Persistence.IdentityData.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Persistence.Repository
{
    public class GenericRepository<TEntity , Tkey> : IGenericRepository<TEntity , Tkey> where TEntity : BaseEntity<Tkey>
    {
        #region 

        protected readonly LinkOIdentityDbContext _dbContext;

        public GenericRepository(LinkOIdentityDbContext context)
        {
            _dbContext = context;
        }

        #endregion

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _dbContext.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(Tkey id)  => 
            await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task AddAsync(TEntity entity) =>
            await _dbContext.Set<TEntity>().AddAsync(entity);
        public void Update(TEntity entity) =>
            _dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) =>
            _dbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, Tkey> specification)
        {
            var Query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification);
            return await Query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, Tkey> specification)
        {
            var Query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();
            return await Query;
        }
    }
}
