using SmartCareBLL.DTOS.AddressDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAllAddressesAsync();
        Task<AddressDTO> GetAddressByIdAsync(int addressId);
        Task<IEnumerable<AddressDTO>> GetAllAddressesByUserIdAsync(int userId);
        Task<AddressDTO> CreateAddressAsync(CreateAddressDTO createAddressDTO);
        Task<bool> DeleteAddressAsync(int addressId);
    }
}
