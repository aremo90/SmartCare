using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;
using SmartCareBLL.ViewModels.Common;

namespace SmartCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<UserViewModel>>.SuccessResponse(users, "Users retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));
            return Ok(ApiResponse<UserViewModel>.SuccessResponse(user, "User retrieved successfully"));
        }

        [HttpGet("byEmail")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));
            return Ok(ApiResponse<UserViewModel>.SuccessResponse(user, "User retrieved successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid data"));

            try
            {
                var updatedUser = await _userService.UpdateAsync(id, model);
                if (updatedUser == null)
                    return NotFound(ApiResponse<string>.FailResponse("User not found"));

                return Ok(ApiResponse<UserViewModel>.SuccessResponse(updatedUser, "User updated successfully"));
            }
            catch (ApplicationException ex)
            {
                return Conflict(ApiResponse<string>.FailResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred"));
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));

            return Ok(ApiResponse<string>.SuccessResponse("User deleted successfully"));
        }
    }
}
