using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IMedicineService
    {
        Task<Result<IEnumerable<MedicineReminderDTO>>> GetReminderByUserId(string email);
        Task<Result<IEnumerable<DeviceReminderDTO>>> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier);
        Task<Result<MedicineReminderDTO>> CreateReminderAsync(string email ,CreateMedicineReminderDTO model);
        Task<bool> DeleteReminderAsync(int id);
        Task UpdateNextReminderDateAsync(int reminderId);
        Task ProcessPastDueRemindersAsync();
    }
}
