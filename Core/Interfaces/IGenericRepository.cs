using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> GetEntityWithSpec(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        //This method will count the number of items available
        Task<int> CountAsync(ISpecification<T> spec);
    }
}