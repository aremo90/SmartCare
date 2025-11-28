using LinkO.Domin.Models.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Models.IdentityModule
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Address? Address { get; set; }
        public Device? Device { get; set; }
        public GpsLocation? GpsLocation { get; set; }
        public ICollection<MedicineReminder> MedicineReminders { get; set; } = new List<MedicineReminder>();

    }
}
