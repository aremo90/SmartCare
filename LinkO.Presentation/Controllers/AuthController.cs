using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.AuthDTOS;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace LinkO.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.LoginAsync(loginDTO);
            return Ok(ApiResponse<UserDTO>.SuccessResponse(result));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _authService.RegisterAsync(registerDTO);
            return Ok(ApiResponse<UserDTO>.SuccessResponse(result));
        }

    }
}
