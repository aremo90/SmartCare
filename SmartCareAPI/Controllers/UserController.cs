using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;

namespace SmartCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // ✅ GET: api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // ✅ GET: api/user/byEmail?email=test@gmail.com
        [HttpGet("byEmail")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // ✅ PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _userService.UpdateAsync(id, model);
                if (!updated)
                    return NotFound("User not found.");

                return NoContent(); // 204
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message }); // duplicate email
            }
        }





        // ✅ DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result)
                return NotFound("User not found.");

            return NoContent();
        }
    }
}
