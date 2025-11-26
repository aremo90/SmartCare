using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.Enum;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Services
{
    public class MedicineReminderService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MedicineReminderService(IUnitOfWork unitOfWork , IMapper mapper , ILogger<MedicineReminder> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MedicineReminderDTO> CreateReminderAsync(CreateMedicineReminderDTO model)
        {
            if (model is null)
            {
                _logger.LogWarning("CreateReminderAsync called with a null model.");
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                _logger.LogInformation("Creating a new medicine reminder for user {UserId}.", model.UserId);

                var medicineEntity = _mapper.Map<MedicineReminder>(model);
                await _unitOfWork.GetRepository<MedicineReminder, int>().AddAsync(medicineEntity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully created medicine reminder with ID {ReminderId} for user {UserId}.", medicineEntity.Id, model.UserId);

                return _mapper.Map<MedicineReminderDTO>(medicineEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a medicine reminder for user {UserId}.", model.UserId);
                throw; // Re-throwing the exception preserves the stack trace and allows higher-level handlers to process it.
            }
        }

        public async Task<bool> DeleteReminderAsync(int id)
        {
            if (id <= 0)
                return false;

            var MedicineRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
            var Medicine = await MedicineRepository.GetByIdAsync(id);

            if (Medicine == null)
                return false;

            MedicineRepository.Delete(Medicine);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MedicineReminderDTO>> GetReminderByUserId(string userId)
        {
            if (userId is null)
                throw new ArgumentException("Invalid user ID.");

            var MedicineRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
            var AllMedicine = await MedicineRepository.GetAllAsync();
            var UserMedicines = AllMedicine.Where(a => a.UserId == userId);

            return _mapper.Map<IEnumerable<MedicineReminderDTO>>(UserMedicines);
        }


        public async Task<IEnumerable<DeviceReminderDTO>> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier)
        {
            if (deviceIdentifier is null)
                throw new ArgumentException("Invalid device identifier.");
            var deviceRepository = _unitOfWork.GetRepository<Device, int>();
            var allDevices =  await deviceRepository.GetAllAsync();
            var device = allDevices.FirstOrDefault(d => d.DeviceIdentifier == deviceIdentifier);
            var UserId = device?.UserId;
            if (UserId is null)
                throw new ArgumentException("Invalid device identifier.");


            var MedicineRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
            var AllMedicine = await MedicineRepository.GetAllAsync();
            var UserMedicines = AllMedicine.Where(a => a.UserId == UserId);
            return _mapper.Map<IEnumerable<DeviceReminderDTO>>(UserMedicines);
        }

        public async Task UpdateNextReminderDateAsync(int reminderId)
        {
            var reminderRepository = _unitOfWork.GetRepository<MedicineReminder , int>();
            var reminder = await reminderRepository.GetByIdAsync(reminderId);

            if (reminder == null)
            {
                return;
            }

            AdvanceReminderDate(reminder);
            reminderRepository.Update(reminder);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ProcessPastDueRemindersAsync()
        {
            var reminderRepository = _unitOfWork.GetRepository<MedicineReminder,int>();
            var allReminders = await reminderRepository.GetAllAsync();
            var now = DateTime.Now;

            var pastDueReminders = allReminders
                .Where(r => new DateTime(r.StartDate, r.ScheduleTime) < now)
                .ToList();

            if (!pastDueReminders.Any())
            {
                return;
            }

            foreach (var reminder in pastDueReminders)
            {
                while (new DateTime(reminder.StartDate, reminder.ScheduleTime) < now)
                {
                    AdvanceReminderDate(reminder);
                }
                reminderRepository.Update(reminder);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        #region Helpers
        private List<DayOfWeek>? ParseCustomDays(string? customDaysString)
        {
            if (string.IsNullOrWhiteSpace(customDaysString))
            {
                return null;
            }
            return customDaysString.Split(',')
                                   .Select(s => Enum.Parse<DayOfWeek>(s, true))
                                   .ToList();
        }
        private void AdvanceReminderDate(MedicineReminder reminder)
        {
            switch (reminder.Frequency)
            {
                case RepeatType.Daily:
                    reminder.StartDate = reminder.StartDate.AddDays(1);
                    break;
                case RepeatType.Weekly:
                    reminder.StartDate = reminder.StartDate.AddDays(7);
                    break;
                case RepeatType.CustomDays:
                    var customDays = ParseCustomDays(reminder.CustomDays);
                    if (customDays == null || !customDays.Any())
                    {
                        return; // Cannot calculate next date, so skip.
                    }

                    var dayIndexes = customDays.Select(d => (int)d).OrderBy(i => i).ToList();
                    var currentDayIndex = (int)reminder.StartDate.DayOfWeek;

                    // Find the index of the next day in the schedule
                    var nextDayIndex = dayIndexes.FirstOrDefault(i => i > currentDayIndex, -1);

                    int daysToAdd;
                    if (nextDayIndex == -1) // No later day in this week, so cycle to the first day of next week
                    {
                        nextDayIndex = dayIndexes.First();
                        daysToAdd = (7 - currentDayIndex) + nextDayIndex;
                    }
                    else // Next day is in the same week
                    {
                        daysToAdd = nextDayIndex - currentDayIndex;
                    }

                    reminder.StartDate = reminder.StartDate.AddDays(daysToAdd);
                    break;
            }
        }


        #endregion
    }
}
