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
using LinkO.Shared.DTOS.AuthDTOS;
using LinkO.Domin.Models.IdentityModule;

namespace SmartCareBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            #region User

            CreateMap<UserInfoDTO , ApplicationUser>().ReverseMap();
            #endregion

            #region Address

            CreateMap<Address, LinkO.Shared.DTOS.AddressDTOS.AddressDTO>();
            CreateMap<CreateAddressDTO, Address>().ReverseMap();
            #endregion
            #region Device

            CreateMap<Device, DeviceDTO>();
            CreateMap<CreateDeviceDTO, Device>().ReverseMap();
            #endregion
            #region Medicine Reminder

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
            #endregion
            #region GPS

            CreateMap<GpsLocation, GpsDTO>().ReverseMap();

            #endregion
            #region Product

            CreateMap<Product , ProductDTO>()
                .ForMember(dest => dest.ProductType , opt => opt.MapFrom(src => src.ProductType.Name));
            CreateMap<Product , AddProductDTO>().ReverseMap();

            CreateMap<TypeDTO , ProductType>().ReverseMap();
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
            #endregion
            #region Order

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address))
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod != null ? s.DeliveryMethod.ShortName : null));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDTO>();
            #endregion
            #region Enums

            // Map between DTO enum and domain enum by casting.
            CreateMap<LinkO.Shared.DTOS.OrderDTOS.AddressDTO, OrderAddress>()
                .ForMember(d => d.PaymentMethod, opt => opt.MapFrom(s => (PaymentMethod)s.PaymentMethod))
                .ReverseMap()
                .ForMember(d => d.PaymentMethod, opt => opt.MapFrom(s => (PaymentMethodDTO)s.PaymentMethod));
            #endregion

        }

    }

}
