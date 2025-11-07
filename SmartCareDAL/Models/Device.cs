using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCareDAL.Models
{
    public class Device : BaseEntity
    {
        
        public string DeviceIdentifier { get; set; } = null!; // e.g., MAC Address or Serial Number
        public string DeviceName { get; set; } = null!;

        // navigation property
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
