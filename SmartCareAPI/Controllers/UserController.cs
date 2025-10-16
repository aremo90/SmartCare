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

        // ✅ POST: api/user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        // ✅ PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateAsync(id, model);
            if (!result)
                return NotFound("User not found.");

            return NoContent();
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
