using SmartCareDAL.Models.Enum;
using System;
using System.Collections.Generic;

namespace SmartCareBLL.DTOS.MedicineReminderDTOS
{
    public class MedicineReminderGroupedDTO
    {
        public List<int> Ids { get; set; } = new List<int>();
        public int UserId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public int Dosage { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string MedicationType { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public List<DayOfWeek>? CustomDays { get; set; }
        public DateOnly StartDate { get; set; }
        public List<TimeOnly>? ScheduleTime { get; set; }
    }
}