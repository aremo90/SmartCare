using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Specification
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        public ICollection<Expression<Func<TEntity , object>>> IncludeExpression { get; }
        public Expression<Func<TEntity,bool>> WhereExpression { get; }
    }
}
