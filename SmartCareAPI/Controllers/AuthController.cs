using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Classes;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;
using SmartCareBLL.ViewModels.LoginViewModel;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST api/auth/signup
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] UserCreateViewModel model)
    {
        try
        {
            var user = await _authService.RegisterAsync(
                model.FirstName, model.LastName, model.Email, model.Password, model.Gender, model.DateOfBirth);

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Gender,
                user.DateOfBirth
            });
        }
        catch (ApplicationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        try
        {
            var token = await _authService.LoginAsync(model.Email, model.Password);
            return Ok(new { token });
        }
        catch (ApplicationException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
