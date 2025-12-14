using LinkO.Shared.CommonResult;
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
        Task<Result<IEnumerable<AddressDTO>>> GetAddressByUserAsync(string Email);
        Task<Result<AddressDTO>> CreateAddressAsync(string Email , CreateAddressDTO createAddressDTO);
        Task<bool> DeleteAddressAsync(int addressId);
    }
}
