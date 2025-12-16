using LinkO.Domin.Models.IdentityModule;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkO.Domin.Models
{
    public class Device : BaseEntity<int>
    {
        
        public string DeviceIdentifier { get; set; } = null!; // e.g., MAC Address or Serial Number
        public string DeviceName { get; set; } = null!;

        // navigation property
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = null!;
    }
}
