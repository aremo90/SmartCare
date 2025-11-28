using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Contract
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        //public ICollection<Expression<Func<TEntity , object>>> IncludeExpression { get; }
        //public Expression<Func<TEntity,bool>> WhereExpression { get; }
    }
}
