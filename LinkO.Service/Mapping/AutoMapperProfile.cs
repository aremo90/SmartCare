using AutoMapper;
using LinkO.Domin.Models;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.DeviceDTOS;
using LinkO.Shared.DTOS.GpsDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCareBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<Address, AddressDTO>();
            CreateMap<CreateAddressDTO, Address>().ReverseMap();

            CreateMap<Device, DeviceDTO>();
            CreateMap<CreateDeviceDTO, Device>().ReverseMap();

            // Medicine Reminder Mappings
            CreateMap<MedicineReminder, MedicineReminderDTO>().ReverseMap();


            CreateMap<CreateMedicineReminderDTO, MedicineReminder>()
                .ForMember(dest => dest.CustomDays, opt => opt.MapFrom(src =>
                    src.CustomDays != null && src.CustomDays.Any()
                        ? string.Join(",", src.CustomDays)
                        : null));

            CreateMap<MedicineReminder, DeviceReminderDTO>()
                .ForMember(dest => dest.ScheduleDate, opt => opt.MapFrom(src => src.StartDate))
                .ReverseMap()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.ScheduleDate));

            CreateMap<GpsLocation, GpsDTO>().ReverseMap();

            CreateMap<Product , ProductDTO>()
                .ForMember(dest => dest.ProductType , opt => opt.MapFrom(src => src.ProductType.Name));

            CreateMap<TypeDTO , ProductType>().ReverseMap();
        }

    }

}
