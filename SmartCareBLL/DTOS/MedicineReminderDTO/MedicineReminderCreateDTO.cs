using SmartCareDAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.DTOS.MedicineReminderDTOS
{
    public class MedicineReminderCreateDTO
    {
        public int UserId { get; set; }
        public string MedicationName { get; set; } = null!;
        public int Dosage { get; set; }
        public string Unit { get; set; } = null!; // e.g., mg, ml, 
        public string MedicationType { get; set; } = null!; //pills / drops / syringe
        public RepeatType Frequency { get; set; }
        public List<DayOfWeek>? CustomDays { get; set; } // Used if Frequency is CustomDays
        public DateOnly StartDate { get; set; } // The calendar date for the reminder
        public List<TimeOnly>? ScheduleTime { get; set; } // The specific time of day for the reminder
    }
}
