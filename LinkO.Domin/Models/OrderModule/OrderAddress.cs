using LinkO.Domin.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Models.OrderModule
{
    public class OrderAddress
    {
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string UserAddress { get; set; } = default!;
        public PaymentMethod PaymentMethod { get; set; }

    }
}
