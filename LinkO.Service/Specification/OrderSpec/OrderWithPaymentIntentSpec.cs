using Linko.Service.Specification;
using LinkO.Domin.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Service.Specification.OrderSpec
{
    public class OrderWithPaymentIntentSpec : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentSpec(string IntentId) : base(O => O.PaymentIntentId == IntentId)
        {

        }
    }
}
