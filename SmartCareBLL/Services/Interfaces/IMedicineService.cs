using SmartCareBLL.DTOS.MedicineReminderDTOS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineReminderDTO>> GetAllReminderAsync();
        Task<IEnumerable<MedicineReminderDTO>> GetReminderByUserIdAsync(int userId);
        Task<MedicineReminderDTO?> GetReminderByIdAsync(int id);
        Task<IEnumerable<MedicineReminderDTO>?> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier);
        Task<MedicineReminderDTO?> CreateReminderAsync(MedicineReminderCreateDTO model);
        Task<bool> DeleteReminderAsync(int id);
    }
}
