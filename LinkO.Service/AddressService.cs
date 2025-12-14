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
using LinkO.Shared.DTOS.MedicineReminderDTOS;
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

            if (createAddressDTO.FullName == "")
                return Error.InvalidCredentials($"FullName Cannot be Empty");

            if (createAddressDTO.PhoneNumber == "")
                return Error.InvalidCredentials($"PhoneNumber Cannot be Empty");

            if (createAddressDTO.UserAddress == "")
                return Error.InvalidCredentials($"UserAddress Cannot be Empty");

            if (createAddressDTO.PaymentMethod == 0)
                return Error.InvalidCredentials($"Please Select PaymentMethod ");

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

        public async Task<Result<IEnumerable<AddressDTO>>> GetAddressByUserAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email);

            var AddressRepository = _unitOfWork.GetRepository<Address, int>();
            var Address = await AddressRepository.GetAllAsync();
            var UserAddress = Address.Where(d => d.UserId == User?.Id);
            if (UserAddress is null)
                return Error.NotFound("Not Found" , "No Address Found For this User");
            var usrAddressDTO = _mapper.Map<IEnumerable<AddressDTO>>(UserAddress);
            return Result<IEnumerable<AddressDTO>>.Ok(usrAddressDTO);
        }
    }
}
