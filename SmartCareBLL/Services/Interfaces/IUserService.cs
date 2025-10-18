using SmartCareBLL.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllAsync();
        Task<UserViewModel?> GetByIdAsync(int id);
        Task<UserViewModel?> GetByEmailAsync(string email);
        Task<UserViewModel?> UpdateAsync(int id, UserUpdateViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
