using LinkO.ServiceAbstraction;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.AuthDTOS;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkO.Presentation.Controllers
{
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.LoginAsync(loginDTO);
            return HandleResult<UserDTO>(result);
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _authService.RegisterAsync(registerDTO);
            return HandleResult<UserDTO>(result);
        }

        [Authorize]
        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result = await _authService.CheckEmailAsync(email);
            return Ok(Result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserInfoDTO>> GetUserInfo()
        {
            var Result = await _authService.GetUserByEmailAsync(GetUserEmail());
            return HandleResult<UserInfoDTO>(Result);
        }

        [Authorize]
        [HttpPost("updateUser")]
        public async Task<ActionResult<UserInfoDTO>> UpdateProfilePicture([FromBody] UpdateUserInfo updateUserInfo)
        {
            var result = await _authService.UpdateUserProfile(GetUserEmail() , updateUserInfo);
            return HandleResult<UserInfoDTO>(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("AllUsers")]
        public async Task<ActionResult<IEnumerable<UserInfoDTO>>> GetAllUsers()
        {
            var result = await _authService.GetAllUsersAsync();
            return HandleResult<IEnumerable<UserInfoDTO>>(result);
        }

    }
}
