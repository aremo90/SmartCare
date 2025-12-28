using LinkO.Domin.Models.IdentityModule;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkO.Domin.Models
{
    public class Device : BaseEntity<int>
    {
        public string DeviceIdentifier { get; set; } = null!;
        public string? DeviceName { get; set; }

        // navigation property
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
