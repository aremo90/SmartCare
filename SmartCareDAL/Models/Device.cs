using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCareDAL.Models
{
    public class Device : BaseEntity
    {
        public int UserId { get; set; }
        public string DeviceIdentifier { get; set; } = null!; // e.g., MAC Address or Serial Number
        public string? Model { get; set; }
        public bool IsActive { get; set; }
        public bool IsPaired { get; set; }
        public double? SignalStrength { get; set; }

        // Navigation
        public User? User { get; set; }
    }
}
