using SmartCareBLL.ViewModels.MedicineViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IMedicineReminderService
    {
        Task<IEnumerable<MedicineReminderViewModel>> GetAllAsync();
        Task<IEnumerable<MedicineReminderViewModel>> GetByUserIdAsync(int userId);
        Task<MedicineReminderViewModel?> GetByIdAsync(int id);
        Task<MedicineReminderViewModel?> CreateAsync(MedicineReminderCreateViewModel model);
        Task<bool> UpdateAsync(int id, MedicineReminderUpdateViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
