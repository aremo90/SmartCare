using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.MedicineViewModel
{
    public class MedicineReminderCreateViewModel
    {
        public int UserId { get; set; }
        public string MedicineName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public DateTime ReminderDate { get; set; }
        public TimeSpan ReminderTime { get; set; }
        public string RepeatType { get; set; } = "Daily"; // default
        public string? CustomDays { get; set; }           // optional
    }
}
