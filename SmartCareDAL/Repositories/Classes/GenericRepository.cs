using Microsoft.EntityFrameworkCore;
using SmartCareDAL.Data.Context;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        #region 

        protected readonly SmartCareDbContext _dbContext;

        public GenericRepository(SmartCareDbContext context)
        {
            _dbContext = context;
        }

        #endregion

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _dbContext.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(int id)  => 
            await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task AddAsync(TEntity entity) =>
            await _dbContext.Set<TEntity>().AddAsync(entity);
        public void Update(TEntity entity) =>
            _dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) =>
            _dbContext.Set<TEntity>().Remove(entity);

    }
}
