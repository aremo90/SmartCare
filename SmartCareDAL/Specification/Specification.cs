using SmartCareDAL.Models;
using SmartCareDAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Specification
{
    public class Specification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];


        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpression.Add(includeExp);
        }

        public Expression<Func<TEntity, bool>> WhereExpression { get; }
        public Specification(Expression<Func<TEntity, bool>> WhereExp)
        {
            WhereExpression = WhereExp;
        }

    }
}
