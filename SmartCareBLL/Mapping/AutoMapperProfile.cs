using AutoMapper;
using SmartCareBLL.DTOS.AddressDTOS;
using SmartCareBLL.DTOS.DeviceDTOS;
using SmartCareBLL.DTOS.GpsDTOS;
using SmartCareBLL.DTOS.MedicineReminderDTOS;
using SmartCareBLL.DTOS.UserDTOS;
using SmartCareDAL.Models;
using System;

namespace SmartCareBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.DeviceIdentifier , opt => opt.MapFrom(src => src.Device.DeviceIdentifier))      
                .ForMember(dest => dest.DeviceName , opt => opt.MapFrom(src => src.Device.DeviceName));      

            CreateMap<Address, AddressDTO>();
            CreateMap<CreateAddressDTO, Address>();
            CreateMap<Address, CreateAddressDTO>();

            CreateMap<Device,DeviceDTO>();
            CreateMap<Device, CreateDeviceDTO>();
            CreateMap<CreateDeviceDTO, Device>();

            CreateMap<MedicineReminder, MedicineReminderDTO>();
            CreateMap<MedicineReminderDTO, MedicineReminder>();

            CreateMap<MedicineReminder, MedicineReminderCreateDTO>();
            CreateMap<MedicineReminder, MedicineReminderCreateDTO>().ReverseMap();

            CreateMap<GpsLocation, GpsDTO>();
            CreateMap<GpsLocation, GpsDTO>().ReverseMap();
        }

    }

}
