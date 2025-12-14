using LinkO.Domin.Models.Enum;
using LinkO.Domin.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Models
{
    public class Address : BaseEntity<int>
    {
        public string UserId { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string UserAddress { get; set; } = default!;
        public PaymentMethod PaymentMethod { get; set; }

        // Navigation property
        public ApplicationUser? User { get; set; } = default!;

    }
}
