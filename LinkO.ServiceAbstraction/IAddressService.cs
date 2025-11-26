using LinkO.Shared.DTOS.AddressDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAllAddressesAsync();
        Task<AddressDTO> GetAddressByIdAsync(int addressId);
        Task<IEnumerable<AddressDTO>> GetAllAddressesByUserIdAsync(string userId);
        Task<AddressDTO> CreateAddressAsync(CreateAddressDTO createAddressDTO);
        Task<bool> DeleteAddressAsync(int addressId);
    }
}
