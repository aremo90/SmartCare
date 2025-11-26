using AutoMapper;
using LinkO.Domin.Models;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.DeviceDTOS;
using LinkO.Shared.DTOS.GpsDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.DTOS.UserDTOS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCareBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();    

            CreateMap<Address, AddressDTO>();
            CreateMap<CreateAddressDTO, Address>().ReverseMap();

            CreateMap<Device,DeviceDTO>();
            CreateMap<CreateDeviceDTO, Device>().ReverseMap();

            // Medicine Reminder Mappings
            CreateMap<MedicineReminder, MedicineReminderDTO>();


            CreateMap<CreateMedicineReminderDTO, MedicineReminder>()
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.CustomDays, opt => opt.MapFrom(src =>
                    src.CustomDays != null && src.CustomDays.Any()
                        ? string.Join(",", src.CustomDays)
                        : null));

            CreateMap<MedicineReminder, DeviceReminderDTO>();

            CreateMap<GpsLocation, GpsDTO>().ReverseMap();
        }

    }

}
