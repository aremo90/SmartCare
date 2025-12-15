using Linko.Service.Specification;
using LinkO.Domin.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Service.Specification.OrderSpec
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string Email) : base(O => O.UserEmail == Email)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
        }

        public OrderSpecification(Guid Id, string Email) : base(O => O.Id == Id
                                 && (string.IsNullOrEmpty(Email) || O.UserEmail.ToLower() == Email.ToLower()))
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
        }
    }
}
