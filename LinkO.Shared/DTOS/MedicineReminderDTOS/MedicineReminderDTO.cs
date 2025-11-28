using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.MedicineReminderDTOS
{
    public class MedicineReminderDTO
    {
        public int Id { get; set; }
        public string MedicationName { get; set; } =  default!;
        public int Dosage { get; set; }
        public string Unit { get; set; } = default!;
        public string MedicationType { get; set; } = default!;
        public string Frequency { get; set; } = default!;
        public string? CustomDays { get; set; }
        public DateOnly StartDate { get; set; }
        public TimeOnly ScheduleTime { get; set; }

    }
}
