using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.AddressViewModel;
using SmartCareBLL.ViewModels.Common;

namespace SmartCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService) => _addressService = addressService;

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
            return Ok(ApiResponse<IEnumerable<AddressViewModel>>.SuccessResponse(addresses, "Addresses retrieved successfully"));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CreateAddressViewModel model)
        {
            var address = await _addressService.AddAddressAsync(
                model.UserId, model.BuildingNumber, model.Street, model.City, model.ZipCode);

            if (address == null)
                return NotFound(ApiResponse<string>.FailResponse($"User with ID {model.UserId} not found."));

            return Ok(ApiResponse<AddressViewModel>.SuccessResponse(address, "Address added successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _addressService.DeleteAddressAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Address deleted successfully"));
        }
    }
}
