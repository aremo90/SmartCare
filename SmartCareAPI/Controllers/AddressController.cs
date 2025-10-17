using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.AddressViewModel;

namespace SmartCareAPI.Controllers
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

        // ✅ GET: api/address/user/5
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
            return Ok(addresses);
        }

        // ✅ POST: api/address
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAddressViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _addressService.AddAddressAsync(model.UserId, model.BuildingNumber, model.Street, model.City);
            return Ok(new { message = "Address added successfully" });
        }

        // ✅ DELETE: api/address/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _addressService.DeleteAddressAsync(id);
            return NoContent();
        }
    }
}
