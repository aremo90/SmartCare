using AutoMapper;
using SmartCareBLL.ViewModels;
using SmartCareDAL.Models;
using SmartCareDAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<User, UserListViewModel>();

            CreateMap<UserCreateViewModel, User>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)))
                    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UserUpdateViewModel, User>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
