using LinkO.Shared.DTOS.EnumDTOS;
using System;
using System.Collections.Generic;

namespace LinkO.Shared.DTOS.MedicineReminderDTOS
{
    public class CreateMedicineReminderDTO
    {
        public string MedicationName { get; set; } = default!;
        public int Dosage { get; set; }
        public string Unit { get; set; } = default!;
        public string MedicationType { get; set; } = default!;
        public RepeatTypeDTO Frequency { get; set; }
        public List<DayOfWeek>? CustomDays { get; set; }
        public DateOnly StartDate { get; set; }
        public TimeOnly ScheduleTime { get; set; }
    }
}
