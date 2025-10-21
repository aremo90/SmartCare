using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.DeviceViewModel
{
    public class DeviceRegisterDto
    {
        public int UserId { get; set; }
        public string DeviceIdentifier { get; set; } = null!;
        public string? Model { get; set; }
    }
}
