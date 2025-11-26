using LinkO.Domin.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Models
{
    public class Address : BaseEntity
    {
        public string UserId { get; set; } = default!;
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public int ZipCode { get; set; }

        // Navigation property
        public ApplicationUser? User { get; set; } = default!;

    }
}
