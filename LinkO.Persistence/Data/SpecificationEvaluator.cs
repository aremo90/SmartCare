using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using Microsoft.EntityFrameworkCore;


namespace LinkO.Persistence.Data
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity , Tkey>(IQueryable<TEntity> EntryPoint,
            ISpecification<TEntity, Tkey> specification) where TEntity : BaseEntity<Tkey>
        {
            var Query = EntryPoint;
            if (specification is not null)
            {
                //Where expression
                if (specification.WhereExpression is not null)
                {
                    Query = Query.Where(specification.WhereExpression);
                }

                //Include expressions
                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any())
                {
                    foreach (var includeExpression in specification.IncludeExpression)
                    {
                        Query = Query.Include(includeExpression);
                    }
                }

            }
            return Query;
        }
    }
}
