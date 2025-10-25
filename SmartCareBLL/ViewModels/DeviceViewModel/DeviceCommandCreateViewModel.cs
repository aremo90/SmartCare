using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.DeviceViewModel
{
    public class DeviceCommandCreateViewModel
    {
        public int UserId { get; set; }
        public string CommandType { get; set; } = null!;
        public string? CommandData { get; set; }
    }
}
