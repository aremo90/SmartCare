using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.DeviceDTOS
{
    public class DeviceDTO
    {
        public int Id { get; set; }
        public string DeviceIdentifier { get; set; } = null!; // e.g., MAC Address or Serial Number
        public string DeviceName { get; set; } = null!;
    }
}
