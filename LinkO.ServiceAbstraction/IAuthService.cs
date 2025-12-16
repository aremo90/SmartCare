using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.AuthDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IAuthService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<bool> CheckEmailAsync(string email);
        Task<Result<UserInfoDTO>> GetUserByEmailAsync(string email);
        Task<Result<UserInfoDTO>> UpdateUserProfile(string email, UpdateUserInfo updateUserInfo);
        Task<Result<IEnumerable<UserInfoDTO>>> GetAllUsersAsync();
    }
}
