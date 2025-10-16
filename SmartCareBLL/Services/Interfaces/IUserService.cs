using SmartCareBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllAsync();
        Task<UserViewModel?> GetByIdAsync(int id);
        Task<UserViewModel?> GetByEmailAsync(string email); // ✅ NEW
        Task<UserViewModel> CreateAsync(UserCreateViewModel model);
        Task<bool> UpdateAsync(int id, UserUpdateViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
