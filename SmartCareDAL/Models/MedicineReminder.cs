using SmartCareDAL.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace SmartCareDAL.Models
{
    public class MedicineReminder : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string MedicineName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Dosage { get; set; } = null!;

        // New fields replacing ReminderTime
        [Required]
        [Column(TypeName = "date")]
        public DateTime ScheduleDate { get; set; } // The calendar date for the reminder

        [Required]
        public TimeSpan ScheduleTime { get; set; } // The specific time of day for the reminder

        [Required]
        public RepeatType RepeatPattern { get; set; } // Daily, Weekly, CustomDays, etc.

        // Optional: store which days apply if RepeatPattern == CustomDays
        public string? DaysOfWeek { get; set; } // e.g., "Mon,Wed,Fri"

        public bool IsTaken { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}