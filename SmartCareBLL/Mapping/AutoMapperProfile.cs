using AutoMapper;
using SmartCareBLL.ViewModels;
using SmartCareBLL.ViewModels.MedicineViewModel;
using SmartCareDAL.Models;
using SmartCareDAL.Models.Enum;
using System;

namespace SmartCareBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserViewModel>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<User, UserListViewModel>();

            CreateMap<UserCreateViewModel, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UserUpdateViewModel, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Medicine Reminder mappings
            CreateMap<MedicineReminderCreateViewModel, MedicineReminder>()
                .ForMember(dest => dest.ScheduleDate, opt => opt.MapFrom(src => src.ReminderDate))
                .ForMember(dest => dest.ScheduleTime, opt => opt.MapFrom(src => src.ReminderTime))
                .ForMember(dest => dest.RepeatPattern, opt => opt.MapFrom(src => ParseRepeatType(src.RepeatType)))
                .ForMember(dest => dest.DaysOfWeek, opt => opt.MapFrom(src => src.CustomDays))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }

        private static RepeatType ParseRepeatType(string repeatType)
        {
            if (Enum.TryParse<RepeatType>(repeatType, true, out var result))
                return result;

            throw new ArgumentException($"Invalid RepeatType value: {repeatType}");
        }
    }

}
