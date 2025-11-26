using SmartCareBLL.DTOS.MedicineReminderDTO;
using SmartCareBLL.DTOS.MedicineReminderDTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineReminderGroupedDTO>> GetAllReminderAsync();
        Task<IEnumerable<MedicineReminderDTO>> GetReminderByUserIdAsync(int userId);
        Task<IEnumerable<MedicineReminderGroupedDTO>> GetReminderByUserIdGroupedAsync(int userId);
        Task<MedicineReminderGroupedDTO?> GetReminderByIdAsync(int id);
        Task<IEnumerable<DeviceReminderDTO>?> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier);
        Task<MedicineReminderGroupedDTO?> CreateReminderAsync(MedicineReminderCreateDTO model);
        Task<bool> DeleteReminderAsync(int id);
        Task UpdateNextReminderDateAsync(int reminderId);
        Task ProcessPastDueRemindersAsync();
    }
}
