using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.MedicineViewModel
{
    public class MedicineReminderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MedicineName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public DateTime ReminderDate { get; set; }  // e.g., 2025-10-18
        public TimeSpan ReminderTime { get; set; }  // e.g., 08:30:00
        public string RepeatType { get; set; } = null!; // "Daily", "Weekly", "Custom", etc.
        public string? CustomDays { get; set; }     // optional, comma-separated (Mon, Wed, Fri)
        public bool IsTaken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
