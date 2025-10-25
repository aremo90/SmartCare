using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.MedicineViewModel
{
    public class MedicineReminderUpdateViewModel
    {
        public string? MedicineName { get; set; }
        public string? Dosage { get; set; }
        public DateTime? ReminderDate { get; set; }
        public TimeSpan? ReminderTime { get; set; }
        public string? RepeatType { get; set; }
        public string? CustomDays { get; set; }
        public bool? IsTaken { get; set; }
    }
}
