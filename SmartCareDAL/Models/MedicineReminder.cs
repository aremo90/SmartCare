using SmartCareDAL.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace SmartCareDAL.Models
{
    public class MedicineReminder : BaseEntity
    {

        public int UserId { get; set; }
        public DateOnly ScheduleDate { get; set; } // The calendar date for the reminder
        public TimeOnly ScheduleTime { get; set; } // The specific time of day for the reminder
        public User? User { get; set; }
    }
}