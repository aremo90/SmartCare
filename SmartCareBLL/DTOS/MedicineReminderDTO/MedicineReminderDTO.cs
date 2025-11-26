using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.DTOS.MedicineReminderDTOS
{
    public class MedicineReminderDTO
    {
        public int Id { get; set; }
        public string MedicationName { get; set; } = null!;
        public int Dosage { get; set; }
        public string Unit { get; set; } = null!; // e.g., mg, ml, pills
        public string MedicationType { get; set; } = null!;
        public string Frequency { get; set; } = string.Empty;
        public List<string>? CustomDays { get; set; }
        public DateOnly StartDate { get; set; } 
        public TimeOnly ScheduleTime { get; set; } 
    }
}
