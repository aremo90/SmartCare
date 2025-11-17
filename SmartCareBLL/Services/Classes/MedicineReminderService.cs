using AutoMapper;
using SmartCareBLL.DTOS.MedicineReminderDTO;
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
        #endregion

        #region get all

        public async Task<IEnumerable<MedicineReminderGroupedDTO>> GetAllReminderAsync()
        {
            var reminders = await _unitOfWork.GetRepository<MedicineReminder>().GetAllAsync();

            var groupedReminders = reminders
                .GroupBy(r => new {
                    r.UserId, r.MedicationName, r.Dosage, r.Unit,
                    r.MedicationType, r.Frequency, r.CustomDays, r.StartDate
                })
                .Select(g => new MedicineReminderGroupedDTO
                {
                    Ids = g.Select(r => r.Id).ToList(),
                    UserId = g.Key.UserId,
                    MedicationName = g.Key.MedicationName,
                    Dosage = g.Key.Dosage,
                    Unit = g.Key.Unit,
                    MedicationType = g.Key.MedicationType,
                    Frequency = g.Key.Frequency.ToString(),
                    CustomDays = ParseCustomDays(g.Key.CustomDays),
                    StartDate = g.Key.StartDate,
                    ScheduleTime = g.Select(r => r.ScheduleTime).OrderBy(t => t).ToList()
                });

            return groupedReminders;
        }
        #endregion
        #region get by User Id

        public async Task<IEnumerable<MedicineReminderGroupedDTO>> GetReminderByUserIdAsync(int userId)
        {
            var allReminders = await _unitOfWork.GetRepository<MedicineReminder>().GetAllAsync();
            var userReminders = allReminders.Where(r => r.UserId == userId);

            var groupedReminders = userReminders
                .GroupBy(r => new {
                    r.MedicationName, r.Dosage, r.Unit,
                    r.MedicationType, r.Frequency, r.CustomDays, r.StartDate
                })
                .Select(g => new MedicineReminderGroupedDTO
                {
                    Ids = g.Select(r => r.Id).ToList(),
                    UserId = userId,
                    MedicationName = g.Key.MedicationName,
                    Dosage = g.Key.Dosage,
                    Unit = g.Key.Unit,
                    MedicationType = g.Key.MedicationType,
                    Frequency = g.Key.Frequency.ToString(),
                    CustomDays = ParseCustomDays(g.Key.CustomDays),
                    StartDate = g.Key.StartDate,
                    ScheduleTime = g.Select(r => r.ScheduleTime).OrderBy(t => t).ToList()
                });

            return groupedReminders;
        }
        #endregion
        #region GetReminder by ReminderID

        public async Task<MedicineReminderGroupedDTO?> GetReminderByIdAsync(int id)
        {
            var reminder = await _unitOfWork.GetRepository<MedicineReminder>().GetByIdAsync(id);
            if (reminder == null)
                return null;

            var allReminders = await _unitOfWork.GetRepository<MedicineReminder>().GetAllAsync();

            var siblingReminders = allReminders.Where(r =>
                r.UserId == reminder.UserId &&
                r.MedicationName == reminder.MedicationName &&
                r.Dosage == reminder.Dosage &&
                r.Unit == reminder.Unit &&
                r.MedicationType == reminder.MedicationType &&
                r.Frequency == reminder.Frequency &&
                r.CustomDays == reminder.CustomDays &&
                r.StartDate == reminder.StartDate
            ).ToList();

            if (!siblingReminders.Any())
                return null;

            return new MedicineReminderGroupedDTO
            {
                Ids = siblingReminders.Select(r => r.Id).ToList(),
                UserId = reminder.UserId,
                MedicationName = reminder.MedicationName,
                Dosage = reminder.Dosage,
                Unit = reminder.Unit,
                MedicationType = reminder.MedicationType,
                Frequency = reminder.Frequency.ToString(),
                CustomDays = ParseCustomDays(reminder.CustomDays),
                StartDate = reminder.StartDate,
                ScheduleTime = siblingReminders.Select(r => r.ScheduleTime).OrderBy(t => t).ToList()
            };
        }
        #endregion

        #region GetBy Device Identifier
        public async Task<IEnumerable<DeviceReminderDTO>?> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier)
        {
            var deviceRepository = _unitOfWork.GetRepository<Device>();
            var allDevices = await deviceRepository.GetAllAsync();
            var device = allDevices.FirstOrDefault(d => d.DeviceIdentifier == deviceIdentifier);

            if (device == null)
            {
                return null;
            }

            var reminderRepository = _unitOfWork.GetRepository<MedicineReminder>();
            var allReminders = await reminderRepository.GetAllAsync();
            var userReminders = allReminders.Where(r => r.UserId == device.UserId && r.IsTaken == false);

            var deviceReminders = userReminders.Select(r => new DeviceReminderDTO
            {
                Id = r.Id,
                ScheduleDate = r.StartDate,
                ScheduleTime = r.ScheduleTime
            }).ToList();

            return deviceReminders;
        }
        #endregion

        #region Create New Reminder

        public async Task<MedicineReminderGroupedDTO?> CreateReminderAsync(MedicineReminderCreateDTO model)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(model.UserId);
            if (user == null)
                throw new ApplicationException($"User with ID {model.UserId} not found.");

            if (model.ScheduleTime == null || !model.ScheduleTime.Any())
                throw new ApplicationException("At least one reminder time must be provided.");

            var createdReminders = new List<MedicineReminder>();
            var reminderRepository = _unitOfWork.GetRepository<MedicineReminder>();
            var customDaysString = model.CustomDays != null ? string.Join(",", model.CustomDays) : null;

            foreach (var time in model.ScheduleTime)
            {
                var reminder = new MedicineReminder
                {
                    UserId = model.UserId,
                    MedicationName = model.MedicationName,
                    Dosage = model.Dosage,
                    Unit = model.Unit,
                    MedicationType = model.MedicationType,
                    Frequency = model.Frequency,
                    CustomDays = customDaysString,
                    StartDate = model.StartDate,
                    ScheduleTime = time
                };

                await reminderRepository.AddAsync(reminder);
                createdReminders.Add(reminder);
            }

            await _unitOfWork.SaveChangesAsync();

            if (!createdReminders.Any())
                return null;
            
            var responseDto = new MedicineReminderGroupedDTO
            {
                Ids = createdReminders.Select(r => r.Id).ToList(),
                UserId = model.UserId,
                MedicationName = model.MedicationName,
                Dosage = model.Dosage,
                Unit = model.Unit,
                MedicationType = model.MedicationType,
                Frequency = model.Frequency.ToString(),
                CustomDays = model.CustomDays,
                StartDate = model.StartDate,
                ScheduleTime = model.ScheduleTime
            };

            return responseDto;
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
