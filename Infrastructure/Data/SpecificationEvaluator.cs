using System.Linq;
using Core.Models;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        ///Returns a query that will be sent to GenericRepository in order to make a call to EF Core.
        /// </summary>
        /// <param name="inputQuery">The query that will accesss EF Core and ask for data in DB</param>
        /// <param name="spec">The Specification is the value of the query, e.g. brandId=2</param>
        /// <returns></returns>
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            if (spec.Criteria != null)
            {
                //The Criteria is received in BaseSpecification constructor and sets the Where clause
                // e.g. spec.Criteria could be p => p.ProductTypeId == id
                inputQuery = inputQuery.Where(spec.Criteria);
            }

            //This block evaluates the OrderBy present in BaseSpecification.cs.
            if (spec.OrderBy != null)
            {
                inputQuery = inputQuery.OrderBy(spec.OrderBy);
            }

            //This block evaluates the OrderByDescending present in BaseSpecification.cs.
            if (spec.OrderByDescending != null)
            {
                inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
            }

            //If the results will be paginated, sets the configuration of Skip and Take.
            if (spec.IsPagingEnabled)
            {
                inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
            }

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

            return inputQuery;
        }
    }
}