using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AddressDTOS;
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

        public AddressService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<AddressDTO> CreateAddressAsync(CreateAddressDTO createAddressDTO)
        {
            var addressEntity = _mapper.Map<Address>(createAddressDTO);
            await _unitOfWork.GetRepository<Address, int>().AddAsync(addressEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AddressDTO>(addressEntity);
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            if (addressId <= 0)
                return false;

            var addressRepository = _unitOfWork.GetRepository<Address, int>();
            var address = await addressRepository.GetByIdAsync(addressId);

            if (address == null)
                return false;

            addressRepository.Delete(address);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<AddressDTO> GetAddressByIdAsync(int addressId)
        {
            if (addressId <= 0)
                throw new ArgumentException("Invalid address ID.");

            var address = await _unitOfWork.GetRepository<Address, int>().GetByIdAsync(addressId);
            return _mapper.Map<AddressDTO>(address);
        }

        public async Task<IEnumerable<AddressDTO>> GetAllAddressesAsync()
        {
            var address = await _unitOfWork.GetRepository<Address, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<AddressDTO>>(address);
        }
        public async Task<IEnumerable<AddressDTO>> GetAllAddressesByUserIdAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentException("Invalid user ID.");

            var addressRepository = _unitOfWork.GetRepository<Address, int>();
            var allAddresses = await addressRepository.GetAllAsync();
            var userAddresses = allAddresses.Where(a => a.UserId == userId);

            return _mapper.Map<IEnumerable<AddressDTO>>(userAddresses);
        }


    }
}
