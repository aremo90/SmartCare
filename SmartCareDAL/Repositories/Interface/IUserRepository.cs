using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

    }
}
