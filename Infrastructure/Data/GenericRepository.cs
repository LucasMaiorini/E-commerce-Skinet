using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// The method calls SpecificationEvaluator's GetQuery static method and send to it all the specifications as a query.
        /// </summary>
        /// <param name="spec">The specifications (conditions) used in a query</param>
        /// <returns>The query</returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        /// <summary>
        /// The method gets specifications and applies it to a query.
        /// </summary>
        /// <param name="spec">The specifications (conditions) used in a query</param>
        /// <returns>A single entity following the specification</returns>
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        /// <summary>
        /// The method gets specifications and applies it to a query.
        /// </summary>
        /// <param name="spec">The specifications (conditions) used in a query</param>
        /// <returns>A list of entities following the specifications</returns>
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        /// <summary>
        /// Counts how many items still available on list
        /// </summary>
        /// <param name="spec"> The specifications (conditions) used in a query </param>
        /// <returns></returns>
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }


        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

    }
}