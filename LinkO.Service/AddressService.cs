using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.Enum;
using LinkO.Domin.Models.IdentityModule;
using LinkO.Service.Exceptions;
using LinkO.ServiceAbstraction;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.AuthDTOS;
using LinkO.Shared.DTOS.EnumDTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddressService(IUnitOfWork unitOfWork , IMapper mapper , UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<Result<AddressDTO>> CreateAddressAsync(string Email , CreateAddressDTO createAddressDTO)
        {
            var User = await _userManager.FindByEmailAsync(Email);

            if (User is null)
                return Error.NotFound("Cannot Find User for this Email !");

            var Address = new Address
            {
                UserId = User.Id,
                Email = Email,
                FullName = createAddressDTO.FullName,
                PhoneNumber = createAddressDTO.PhoneNumber,
                UserAddress = createAddressDTO.UserAddress,
                PaymentMethod = Enum.Parse<PaymentMethod>(createAddressDTO.PaymentMethod.ToString())
            };

            await _unitOfWork.GetRepository<Address, int>().AddAsync(Address);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AddressDTO>(Address);
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            var addressRepository = _unitOfWork.GetRepository<Address, int>();
            var address = await addressRepository.GetByIdAsync(addressId);

            if (address is null)
                return false;

            addressRepository.Delete(address);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Result<AddressDTO>> GetAddressByUserAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email);
            var AddressRepository = _unitOfWork.GetRepository<Address, int>();
            var Address = (await AddressRepository.GetAllAsync()).FirstOrDefault(d => d.UserId == User.Id);
            if (Address is null)
                return Error.NotFound("Not Found" , "No Address Found");
            return _mapper.Map<AddressDTO>(Address);
        }

    }
}
