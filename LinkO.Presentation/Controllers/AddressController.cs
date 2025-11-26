using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }


        #region Get All Address

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();
                return Ok(ApiResponse<IEnumerable<AddressDTO>>.SuccessResponse(addresses));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }



        #endregion
        #region get address by AddressId

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            try
            {
                var address = await _addressService.GetAddressByIdAsync(id);
                if (address == null)
                {
                    return NotFound(ApiResponse<AddressDTO>.FailResponse($"Address with ID {id} not found."));
                }
                return Ok(ApiResponse<AddressDTO>.SuccessResponse(address));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }


        #endregion

        #region get address by UserId

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllAddressesByUserId(string userId)
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesByUserIdAsync(userId);
                return Ok(ApiResponse<IEnumerable<AddressDTO>>.SuccessResponse(addresses));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        #endregion

        #region create New Address

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDTO createAddressDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailResponse("Invalid model state."));
            }

            try
            {
                var createdAddress = await _addressService.CreateAddressAsync(createAddressDTO);
                if (createdAddress == null)
                {
                    return BadRequest(ApiResponse<AddressDTO>.FailResponse("Failed to create address."));
                }
                return CreatedAtAction(nameof(GetAddressById), new { id = createdAddress.Id }, ApiResponse<AddressDTO>.SuccessResponse(createdAddress));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred while creating the address."));
            }
        }


        #endregion


        #region Delete Address

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var result = await _addressService.DeleteAddressAsync(id);
                if (!result)
                {
                    return NotFound(ApiResponse<string>.FailResponse($"Address with ID {id} not found."));
                }
                return Ok(ApiResponse<string>.SuccessResponse("Address deleted successfully."));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred while deleting the address."));
            }
        }

        #endregion
    }
}
