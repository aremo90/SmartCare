using AutoMapper;
using FirebaseAdmin.Messaging;
using Linko.Service.Specification;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.Enum;
using LinkO.Domin.Models.IdentityModule;
using LinkO.Service.Exceptions;
using LinkO.ServiceAbstraction;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public MedicineReminderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MedicineReminder> logger, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<MedicineReminderDTO>> CreateReminderAsync(string email, CreateMedicineReminderDTO model)
        {
            var User = await _userManager.FindByEmailAsync(email);

            if (User == null)
                return Error.NotFound("User Not Found");

            if (model is null)
                return Error.InvalidCredentials();

            var medicineEntity = _mapper.Map<MedicineReminder>(model);
            medicineEntity.UserId = User.Id;
            await _unitOfWork.GetRepository<MedicineReminder, int>().AddAsync(medicineEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MedicineReminderDTO>(medicineEntity);

        }

        public async Task<bool> DeleteReminderAsync(int id)
        {
            var MedicineRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
            var Medicine = await MedicineRepository.GetByIdAsync(id);

            if (Medicine == null)
                throw new MedicineNotFoundException(id);

            MedicineRepository.Delete(Medicine);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Result<IEnumerable<MedicineReminderDTO>>> GetReminderByUserId(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                return Result<IEnumerable<MedicineReminderDTO>>.Fail(Error.NotFound("User Not found"));

            var MedicineRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
            var MedicineSpec = new BaseSpecification<MedicineReminder, int>(m => m.UserId == User.Id);
            var UserMedicines = await MedicineRepository.GetAllAsync(MedicineSpec);

            var userMedicinesDto = _mapper.Map<IEnumerable<MedicineReminderDTO>>(UserMedicines);
            return Result<IEnumerable<MedicineReminderDTO>>.Ok(userMedicinesDto);
        }


        public async Task<Result<IEnumerable<DeviceReminderDTO>>> GetRemindersByDeviceIdentifierAsync(string deviceIdentifier)
        {
            var deviceRepository = _unitOfWork.GetRepository<Device, int>();
            var spec = new BaseSpecification<Device, int>(d => d.DeviceIdentifier == deviceIdentifier);
            var device = await deviceRepository.GetByIdAsync(spec);

            if (device is null)
                return Result<IEnumerable<DeviceReminderDTO>>.Fail(Error.NotFound("Device not found."));

            var UserId = device.UserId;
            if (UserId is null)
                return Result<IEnumerable<DeviceReminderDTO>>.Fail(Error.NotFound("No User Paired With This Device"));


            var MedicineRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
            var MedicineSpec = new BaseSpecification<MedicineReminder, int>(m => m.UserId == UserId);
            var UserMedicines = await MedicineRepository.GetAllAsync(MedicineSpec);
            if (!UserMedicines.Any())
                return Error.NotFound("No Medicines Found For The User Paired With This Device");

            var deviceRemindersDto = _mapper.Map<IEnumerable<DeviceReminderDTO>>(UserMedicines);
            return Result<IEnumerable<DeviceReminderDTO>>.Ok(deviceRemindersDto);
        }
        public async Task UpdateNextReminderDateAsync(int reminderId)
        {
            var reminderRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
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
            var reminderRepository = _unitOfWork.GetRepository<MedicineReminder, int>();
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


        public async Task<Result<string>> SendNotificationAsync()
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);
            var reminderRepository = _unitOfWork.GetRepository<MedicineReminder, int>();

            // Use specification to load only reminders due right now
            var spec = new BaseSpecification<MedicineReminder, int>(r =>
                r.ScheduleTime.Hour == now.Hour &&
                r.ScheduleTime.Minute == now.Minute &&
                r.StartDate <= today);

            var dueReminders = await reminderRepository.GetAllAsync(spec);

            if (!dueReminders.Any())
                return Result<string>.Fail(Error.NotFound("No medicines due at this time."));

            var userIds = dueReminders
                .Select(r => r.UserId)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            int sent = 0;
            int failed = 0;

            foreach (var uid in userIds)
            {
                var user = await _userManager.FindByIdAsync(uid);
                if (user == null) { failed++; continue; }

                var token = user.DeviceFcmToken;
                if (string.IsNullOrWhiteSpace(token)) { failed++; continue; }

                var message = new Message
                {
                    Token = token,
                    Notification = new Notification
                    {
                        Title = "Medication Time",
                        Body = "Beeb Beeb Medication time has arrived"
                    },
                    Android = new AndroidConfig
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification
                        {
                            Sound = "default",
                            ChannelId = "emergency_channel"
                        }
                    }
                };

                try
                {
                    await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    sent++;
                }
                catch (FirebaseMessagingException ex)
                {
                    if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered || ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
                    {
                        user.DeviceFcmToken = null;
                        await _userManager.UpdateAsync(user);
                    }
                    failed++;
                }
            }

            return Result<string>.Ok($"Notifications sent: {sent}, failed: {failed}");
        }


        #endregion
    }
}
