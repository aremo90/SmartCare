using SmartCareDAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels
{
    public class UserCreateViewModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; // hashed later in service
        public string Gender { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }
}
