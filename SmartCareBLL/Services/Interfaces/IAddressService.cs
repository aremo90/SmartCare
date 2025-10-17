using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IAddressService
    {
        Task AddAddressAsync(int userId, int buildingNumber, string street, string city);
        Task<IEnumerable<ViewModels.AddressViewModel.AddressViewModel>> GetAddressesByUserIdAsync(int userId);
        Task<ViewModels.AddressViewModel.AddressViewModel> GetAddressByIdAsync(int addressId);
        Task DeleteAddressAsync(int addressId);
    }
}
