using SmartCareBLL.ViewModels.AddressViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressViewModel?> AddAddressAsync(int userId, int buildingNumber, string street, string city, int zipCode);
        Task<IEnumerable<ViewModels.AddressViewModel.AddressViewModel>> GetAddressesByUserIdAsync(int userId);
        Task<ViewModels.AddressViewModel.AddressViewModel> GetAddressByIdAsync(int addressId);
        Task DeleteAddressAsync(int addressId);
    }
}
