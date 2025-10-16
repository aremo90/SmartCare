using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Interface
{
    public interface IMedicineReminderRepository : IGenericRepository<MedicineReminder>
    {
        Task<IEnumerable<MedicineReminder>> GetRemindersByUserIdAsync(int userId);

    }
}
