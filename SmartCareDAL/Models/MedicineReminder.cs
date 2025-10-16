using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Models
{
    public class MedicineReminder : BaseEntity
    {
        public int UserId { get; set; }

        public string MedicineName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public DateTime ReminderTime { get; set; }
        public bool IsTaken { get; set; }


        // Navigation property
        public User? User { get; set; }
    }
}
