using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.DeviceViewModel
{
    public class DeviceCommandViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CommandType { get; set; } = null!;
        public string? CommandData { get; set; }
        public bool IsExecuted { get; set; }
        public DateTime? ExecutedAt { get; set; }


    }
}
