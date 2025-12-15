using AutoMapper;
using LinkO.Domin.Models;
using LinkO.Domin.Models.BasketModule;
using LinkO.Domin.Models.OrderModule;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.BasketDTOS;
using LinkO.Shared.DTOS.DeviceDTOS;
using LinkO.Shared.DTOS.GpsDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.DTOS.OrderDTOS;
using LinkO.Shared.DTOS.ProductDTOS;
using LinkO.Shared.DTOS.EnumDTOS;
using LinkO.Domin.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCareBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<Address, LinkO.Shared.DTOS.AddressDTOS.AddressDTO>();
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
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();


            // Map between DTO enum and domain enum by casting.
            CreateMap<LinkO.Shared.DTOS.OrderDTOS.AddressDTO, OrderAddress>()
                .ForMember(d => d.PaymentMethod, opt => opt.MapFrom(s => (PaymentMethod)s.PaymentMethod))
                .ReverseMap()
                .ForMember(d => d.PaymentMethod, opt => opt.MapFrom(s => (PaymentMethodDTO)s.PaymentMethod));

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName)).ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ReverseMap();


            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address));

            CreateMap<DeliveryMethod, DeliveryMethodDTO>();

        }

    }

}
