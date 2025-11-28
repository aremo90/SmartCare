using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Persistence.IdentityData.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];
        private readonly LinkOIdentityDbContext _dbContext;

        public UnitOfWork(LinkOIdentityDbContext dbContext )
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType , out var repository))
            {
                return (IGenericRepository<TEntity, TKey>)repository;
            }
            var newRepository = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories[EntityType] = newRepository;
            return newRepository;
        }

        public async Task<int> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();

    }
}
