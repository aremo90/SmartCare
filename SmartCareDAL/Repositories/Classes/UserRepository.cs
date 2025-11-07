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
