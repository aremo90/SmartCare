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
    public class UserRepository : GenericRepository<User>, IUserRepository 
    {
        public UserRepository(SmartCareDbContext context) : base(context) { }

        public async Task<User?> GetUserWithAddressesAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserWithRemindersAsync(int id)
        {
            return await _context.Users
                .Include(u => u.MedicineReminders)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
