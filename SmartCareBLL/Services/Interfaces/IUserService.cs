using SmartCareBLL.DTOS.UserDTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> DeleteUserByIdAsync(int id);
        Task<UserDTO> UpdateUserByIdAsync(int id, UserToUpdateDTO userDto);
    }
}
