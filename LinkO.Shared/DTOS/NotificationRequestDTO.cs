using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS
{
    public class NotificationRequestDTO
    {
        public string DeviceIdentifier { get; set; } = null!;
        public bool FallDetected { get; set; }
    }
}
