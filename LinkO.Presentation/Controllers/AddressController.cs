using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers 
{
    public class AddressController : ApiBaseController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }



        #region get User address
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var Result = await _addressService.GetAddressByUserAsync(GetUserEmail());
            return HandleResult<AddressDTO>(Result);
        }
        #endregion


        #region create New Address
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AddressDTO>> CreateAddress([FromBody] CreateAddressDTO createAddressDTO)
        {
            var createdAddress = await _addressService.CreateAddressAsync(GetUserEmail(), createAddressDTO);
            return HandleResult<AddressDTO>(createdAddress);
        }
        #endregion


        #region Delete Address
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<AddressDTO>> DeleteAddress(int id)
        {
            var result = await _addressService.DeleteAddressAsync(id);
            return Ok(result);
        }
        #endregion
    }
}
