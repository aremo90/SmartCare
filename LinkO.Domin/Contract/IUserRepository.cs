using LinkO.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Contract
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

    }
}
