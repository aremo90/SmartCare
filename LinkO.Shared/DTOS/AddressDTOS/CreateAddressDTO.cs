using LinkO.Shared.DTOS.EnumDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.AddressDTOS
{
    public class CreateAddressDTO
    {
        public string? UserId { get; set;}
        public string? Email { get; set; }
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string UserAddress { get; set; } = default!;
        public PaymentMethodDTO PaymentMethod { get; set; }
    }
}
