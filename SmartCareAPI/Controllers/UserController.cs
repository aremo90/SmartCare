using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.DTOS.UserDTOS;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels;
using SmartCareBLL.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        #region Get All Users

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(ApiResponse<IEnumerable<UserDTO>>.SuccessResponse(users, "Users retrieved successfully"));
        }
        #endregion

        #region Get user By Id

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));
            return Ok(ApiResponse<UserDTO>.SuccessResponse(user, "User retrieved successfully"));
        }
        #endregion

        #region Update User Info

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserToUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid data"));

            try
            {
                var updatedUser = await _userService.UpdateUserByIdAsync(id, model);
                if (updatedUser == null)
                    return NotFound(ApiResponse<string>.FailResponse("User not found"));

                return Ok(ApiResponse<UserDTO>.SuccessResponse(updatedUser, "User updated successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred"));
            }
        }

        #endregion

        #region Delete User

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));

            return Ok(ApiResponse<string>.SuccessResponse("User deleted successfully"));
        }
        #endregion
    }
}
