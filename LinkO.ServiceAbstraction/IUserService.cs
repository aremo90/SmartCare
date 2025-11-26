using LinkO.Shared.DTOS.UserDTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<UserDTO> DeleteUserByIdAsync(string id);
        Task<UserDTO> UpdateUserByIdAsync(string id, UserToUpdateDTO userDto);
    }
}
