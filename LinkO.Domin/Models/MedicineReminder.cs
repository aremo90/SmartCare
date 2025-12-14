using LinkO.Domin.Models.Enum;
using LinkO.Domin.Models.IdentityModule;
using System;

namespace LinkO.Domin.Models
{
    public class MedicineReminder : BaseEntity<int>
    {

        public string MedicationName { get; set; } = string.Empty;
        public int Dosage { get; set; }
        public string Unit { get; set; } = string.Empty; // e.g., mg, ml
        public string MedicationType { get; set; } = string.Empty; // e.g., pills, drops, syringe
        public RepeatType Frequency { get; set; }
        public string? CustomDays { get; set; } // Stored as comma-separated string e.g., "Monday,Tuesday"
        public DateOnly StartDate { get; set; }
        public TimeOnly ScheduleTime { get; set; }
        public bool IsTaken { get; set; }

        // Nav proprtey
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = null!;
    }
}