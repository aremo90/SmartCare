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
    public class MedicineReminderRepository : GenericRepository<MedicineReminder>, IMedicineReminderRepository
    {
        public MedicineReminderRepository(SmartCareDbContext context) : base(context) { }

        public async Task<IEnumerable<MedicineReminder>> GetRemindersByUserIdAsync(int userId)
        {
            return await _context.MedicineReminders
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
