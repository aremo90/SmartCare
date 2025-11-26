using LinkO.Shared.DTOS.MedicineReminderDTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineReminderDTO>> GetReminderByUserId(string userId);
        Task<IEnumerable<DeviceReminderDTO>> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier);
        Task<MedicineReminderDTO> CreateReminderAsync(CreateMedicineReminderDTO model);
        Task<bool> DeleteReminderAsync(int id);
        Task UpdateNextReminderDateAsync(int reminderId);
        Task ProcessPastDueRemindersAsync();
    }
}
