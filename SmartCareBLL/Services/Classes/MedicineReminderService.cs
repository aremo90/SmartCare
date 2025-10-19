using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.MedicineViewModel;
using SmartCareDAL.Models;
using SmartCareDAL.Models.Enum;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Classes
{
    public class MedicineReminderService : IMedicineReminderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineReminderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ✅ Get all reminders
        public async Task<IEnumerable<MedicineReminderViewModel>> GetAllAsync()
        {
            var reminders = await _unitOfWork.MedicineReminders.GetAllAsync();
            return reminders.Select(MapToViewModel);
        }

        // ✅ Get reminders by user
        public async Task<IEnumerable<MedicineReminderViewModel>> GetByUserIdAsync(int userId)
        {
            var reminders = await _unitOfWork.MedicineReminders.FindAsync(r => r.UserId == userId);
            return reminders.Select(MapToViewModel);
        }

        // ✅ Get single reminder by ID
        public async Task<MedicineReminderViewModel?> GetByIdAsync(int id)
        {
            var reminder = await _unitOfWork.MedicineReminders.GetByIdAsync(id);
            return reminder == null ? null : MapToViewModel(reminder);
        }

        // ✅ Create new reminder
        public async Task<MedicineReminderViewModel?> CreateAsync(MedicineReminderCreateViewModel model)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(model.UserId);
            if (user == null)
                throw new ApplicationException($"User with ID {model.UserId} not found.");

            var entity = new MedicineReminder
            {
                UserId = model.UserId,
                MedicineName = model.MedicineName,
                Dosage = model.Dosage,
                ScheduleDate = model.ReminderDate.Date,
                ScheduleTime = model.ReminderTime,
                RepeatPattern = Enum.TryParse<RepeatType>(model.RepeatType, true, out var parsed)
                    ? parsed
                    : throw new ApplicationException($"Invalid RepeatType '{model.RepeatType}'. Must be one of: {string.Join(", ", Enum.GetNames(typeof(RepeatType)))}"),
                DaysOfWeek = model.CustomDays,
                IsTaken = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.MedicineReminders.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return MapToViewModel(entity);
        }

        // ✅ Update reminder
        public async Task<bool> UpdateAsync(int id, MedicineReminderUpdateViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var reminder = await _unitOfWork.MedicineReminders.GetByIdAsync(id);
            if (reminder == null)
                return false;

            // Update basic info
            if (!string.IsNullOrWhiteSpace(model.MedicineName))
                reminder.MedicineName = model.MedicineName;

            if (!string.IsNullOrWhiteSpace(model.Dosage))
                reminder.Dosage = model.Dosage;

            // Date & time
            if (model.ReminderDate.HasValue)
                reminder.ScheduleDate = model.ReminderDate.Value.Date;

            if (model.ReminderTime.HasValue)
                reminder.ScheduleTime = model.ReminderTime.Value;

            // Repeat type → enum
            if (!string.IsNullOrWhiteSpace(model.RepeatType))
            {
                if (Enum.TryParse<RepeatType>(model.RepeatType, true, out var parsed))
                    reminder.RepeatPattern = parsed;
                else
                    throw new ApplicationException($"Invalid RepeatType '{model.RepeatType}'. Must be one of: {string.Join(", ", Enum.GetNames(typeof(RepeatType)))}");
            }

            // Days of week for custom patterns
            if (!string.IsNullOrWhiteSpace(model.CustomDays))
                reminder.DaysOfWeek = model.CustomDays;

            // Mark as taken or not
            if (model.IsTaken.HasValue)
                reminder.IsTaken = model.IsTaken.Value;

            // Timestamp & tracking
            reminder.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.MedicineReminders.Update(reminder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // ✅ Delete reminder
        public async Task<bool> DeleteAsync(int id)
        {
            var reminder = await _unitOfWork.MedicineReminders.GetByIdAsync(id);
            if (reminder == null)
                return false;

            _unitOfWork.MedicineReminders.Delete(reminder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // 🔁 Manual mapper (entity → view model)
        private static MedicineReminderViewModel MapToViewModel(MedicineReminder r)
        {
            return new MedicineReminderViewModel
            {
                Id = r.Id,
                UserId = r.UserId,
                MedicineName = r.MedicineName,
                Dosage = r.Dosage,
                ReminderDate = r.ScheduleDate,
                ReminderTime = r.ScheduleTime,
                RepeatType = r.RepeatPattern.ToString(),
                CustomDays = r.DaysOfWeek,
                IsTaken = r.IsTaken,
                CreatedAt = r.CreatedAt,
            };
        }
    }
}
