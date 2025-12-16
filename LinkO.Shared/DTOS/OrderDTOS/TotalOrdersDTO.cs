using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.OrderDTOS
{
    public class TotalOrdersDTO
    {
        public int TotalOrdersCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
