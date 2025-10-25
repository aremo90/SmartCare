using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.DeviceViewModel
{
    public class DeviceStatusDto
    {
        public string DeviceIdentifier { get; set; } = null!;
        public bool IsActive { get; set; }
        public double? SignalStrength { get; set; }
    }
}
