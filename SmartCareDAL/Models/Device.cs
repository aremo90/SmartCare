using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCareDAL.Models
{
    public class Device : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DeviceIdentifier { get; set; } = null!; // e.g., MAC Address or Serial Number

        [MaxLength(100)]
        public string? Model { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Split LastSeen into separate date and time fields
        [Column(TypeName = "date")]
        public DateTime? LastSeenDate { get; set; }

        public TimeSpan? LastSeenTime { get; set; }

        public bool IsPaired { get; set; }

        // Optional additional info (battery, signal, etc.)
        public double? BatteryLevel { get; set; }
        public double? SignalStrength { get; set; }

        // Navigation
        public User? User { get; set; }
    }
}
