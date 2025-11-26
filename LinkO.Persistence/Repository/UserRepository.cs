using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Persistence.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartCareDbContext _dbContext;

        public UserRepository(SmartCareDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    }
}
