using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Models
{
    public class DeviceCommand : BaseEntity<int>
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CommandType { get; set; } = null!; // e.g., "Beep", "Vibrate", etc.

        [MaxLength(255)]
        public string? CommandData { get; set; } // e.g., duration or intensity

        [Required]
        public bool IsExecuted { get; set; } = false;

        public DateTime? ExecutedAt { get; set; }

        // Relationship
    }
}
