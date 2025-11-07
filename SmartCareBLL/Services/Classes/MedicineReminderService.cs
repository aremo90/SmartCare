using AutoMapper;
using SmartCareBLL.DTOS.MedicineReminderDTOS;
using SmartCareBLL.DTOS.UserDTOS;
using SmartCareBLL.Services.Interfaces;
using SmartCareDAL.Models;
using SmartCareDAL.Models.Enum;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Classes
{
    public class MedicineReminderService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MedicineReminderService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region get all

        public async Task<IEnumerable<MedicineReminderDTO>> GetAllReminderAsync()
        {
            var reminders = await _unitOfWork.GetRepository<MedicineReminder>().GetAllAsync();
            return _mapper.Map<IEnumerable<MedicineReminderDTO>>(reminders);
        }
        #endregion
        #region get by User Id

        public async Task<IEnumerable<MedicineReminderDTO>> GetReminderByUserIdAsync(int userId)
        {
            var allReminders = await _unitOfWork.GetRepository<MedicineReminder>().GetAllAsync();
            var reminders = allReminders.Where(r => r.UserId == userId);
            return _mapper.Map<IEnumerable<MedicineReminderDTO>>(reminders);
        }
        #endregion
        #region GetReminder by ReminderID

        public async Task<MedicineReminderDTO?> GetReminderByIdAsync(int id)
        {
            var reminder = await _unitOfWork.GetRepository<MedicineReminder>().GetByIdAsync(id);
            return _mapper.Map<MedicineReminderDTO>(reminder);
        }
        #endregion

        #region GetBy Device Identifier
        public async Task<IEnumerable<MedicineReminderDTO>?> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier)
        {
            var deviceRepository = _unitOfWork.GetRepository<Device>();
            var allDevices = await deviceRepository.GetAllAsync();
            var device = allDevices.FirstOrDefault(d => d.DeviceIdentifier == deviceIdentifier);

            if (device == null)
            {
                return null;
            }

            return await GetReminderByUserIdAsync(device.UserId);
        }

        #endregion

        #region Create New Reminder

        public async Task<MedicineReminderDTO?> CreateReminderAsync(MedicineReminderCreateDTO model)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(model.UserId);
            if (user == null)
                throw new ApplicationException($"User with ID {model.UserId} not found.");

            var reminder = _mapper.Map<MedicineReminder>(model);

            await _unitOfWork.GetRepository<MedicineReminder>().AddAsync(reminder);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MedicineReminderDTO>(reminder);
        }
        #endregion
        #region Delete

        public async Task<bool> DeleteReminderAsync(int id)
        {
            var reminder = await _unitOfWork.GetRepository<MedicineReminder>().GetByIdAsync(id);
            if (reminder == null)
                return false;

            _unitOfWork.GetRepository<MedicineReminder>().Delete(reminder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        #endregion


    }
}
