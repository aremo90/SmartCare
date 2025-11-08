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
        public DateOnly ScheduleDate { get; set; } 
        public TimeOnly ScheduleTime { get; set; } 
    }
}
