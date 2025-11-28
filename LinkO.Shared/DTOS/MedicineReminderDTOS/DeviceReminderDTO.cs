using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.MedicineReminderDTOS
{
    public class DeviceReminderDTO
    {
        public int Id { get; set; }
        public DateOnly ScheduleDate { get; set; }
        public TimeOnly ScheduleTime { get; set; }
        public bool IsTaken { get; set; }
    }
}
