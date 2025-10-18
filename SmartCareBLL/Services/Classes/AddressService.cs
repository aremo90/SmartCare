using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.AddressViewModel;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Classes
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressViewModel> GetAddressByIdAsync(int addressId)
        {
            var address = await _unitOfWork.GetRepository<Address>().GetByIdAsync(addressId);
            if (address == null) return null;

            return new AddressViewModel
            {
                Id = address.Id,
                UserId = address.UserId,
                BuildingNumber = address.BuildingNumber,
                Street = address.Street,
                City = address.City,
                ZipCode = address.ZipCode
            };
        }

        public async Task<IEnumerable<AddressViewModel>> GetAddressesByUserIdAsync(int userId)
        {
            var addresses = await _unitOfWork.GetRepository<Address>().FindAsync(a => a.UserId == userId);
            return addresses.Select(a => new AddressViewModel
            {
                Id = a.Id,
                UserId = a.UserId,
                BuildingNumber = a.BuildingNumber,
                Street = a.Street,
                City = a.City,
                ZipCode = a.ZipCode
            });
        }


        public async Task DeleteAddressAsync(int addressId)
        {
            var address = await _unitOfWork.GetRepository<Address>().GetByIdAsync(addressId);
            if (address == null) return;

            _unitOfWork.GetRepository<Address>().Delete(address);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<AddressViewModel?> AddAddressAsync(int userId, int buildingNumber, string street, string city, int zipCode)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return null; // user doesn't exist

            var address = new Address
            {
                UserId = userId,
                BuildingNumber = buildingNumber,
                Street = street,
                City = city,
                ZipCode = zipCode
            };

            await _unitOfWork.GetRepository<Address>().AddAsync(address);
            await _unitOfWork.SaveChangesAsync();

            return new AddressViewModel
            {
                Id = address.Id,
                UserId = address.UserId,
                BuildingNumber = address.BuildingNumber,
                Street = address.Street,
                City = address.City,
                ZipCode = address.ZipCode
            };
        }
    }
}
