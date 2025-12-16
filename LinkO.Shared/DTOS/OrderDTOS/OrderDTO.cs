using LinkO.Shared.DTOS.AddressDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.OrderDTOS
{
    public class OrderDTO
    {
        public string? BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO Address { get; set; }
    }
}
