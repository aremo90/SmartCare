using Microsoft.EntityFrameworkCore;
using SmartCareDAL.Models;
using SmartCareDAL.Specification;


namespace SmartCareDAL.Data.Evaluator
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> EntryPoint ,
            ISpecification<TEntity> specification) where TEntity : BaseEntity
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
