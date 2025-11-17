using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.DTOS.GpsDTOS
{
    public class GpsDTO
    {
        public int UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
