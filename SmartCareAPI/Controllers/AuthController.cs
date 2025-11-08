using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.AuthDTOS.LoginViewDTO;
using SmartCareBLL.DTOS.AuthDTOS;
using SmartCareBLL.DTOS.UserDTOS;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;
using SmartCareBLL.ViewModels.Common;
using SmartCareBLL.ViewModels.LoginViewModel;
using SmartCareDAL.Models;

namespace SmartCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignUpDTO model)
        {
            try
            {
                var user = await _authService.RegisterAsync(
                    model.FirstName, model.LastName, model.Email,
                    model.Password, model.Gender.ToString(), model.DateOfBirth, model.PhoneNumber);

                var responseData = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Gender,
                    user.DateOfBirth,
                    user.PhoneNumber
                };

                return Ok(ApiResponse<object>.SuccessResponse(responseData, "Account created successfully"));
            }
            catch (ApplicationException ex)
            {
                return Conflict(ApiResponse<string>.FailResponse(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewDTO model)
        {
            try
            {
                var result = await _authService.LoginAsync(model.Email, model.Password);

                var data = new
                {
                    UserID = result.UserID,
                    Token = result.Token
                };

                return Ok(ApiResponse<object>.SuccessResponse(data, "Logged in successfully"));
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ApiResponse<string>.FailResponse(ex.Message));
            }
        }
    }
}
