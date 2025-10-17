using SmartCareDAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; } = null!;
        [Required(ErrorMessage = "DateOfBirth is Required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; } = null!;
    }
}
