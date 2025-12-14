using Linko.Service.Specification;
using LinkO.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Service.Specification.ProductSpec
{
    internal class ProductWithTypeSpecification : BaseSpecification<Product, int>
    {

        public ProductWithTypeSpecification() : base(null)
        {
            AddInclude(p => p.ProductType);
        }

        public ProductWithTypeSpecification(int id) : base(P => P.Id == id)
        {
            AddInclude(p => p.ProductType);
        }
    }
}
