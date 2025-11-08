using SmartCareDAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartCareDAL.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;


        // Navigation properties
        public Device? Device { get; set; }
        public GpsLocation? GpsLocation { get; set; }
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<MedicineReminder>? MedicineReminders { get; set; }
    }

}
