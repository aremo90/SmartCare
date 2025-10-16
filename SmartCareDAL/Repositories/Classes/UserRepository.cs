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
        private readonly SmartCareDbContext _context;

        public UserRepository(SmartCareDbContext context) : base(context) 
        {
            _context = context;
            
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

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
