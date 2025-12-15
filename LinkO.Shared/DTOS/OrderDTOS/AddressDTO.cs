using LinkO.Shared.DTOS.EnumDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.OrderDTOS
{
    public class AddressDTO
    {
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string UserAddress { get; set; } = default!;
        public PaymentMethodDTO PaymentMethod { get; set; }
    }
}
