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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartCareDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(SmartCareDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType , out var repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }
            var newRepository = new GenericRepository<TEntity>(_dbContext);
            _repositories[EntityType] = newRepository;
            return newRepository;
        }

        public async Task<int> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();

    }
}
