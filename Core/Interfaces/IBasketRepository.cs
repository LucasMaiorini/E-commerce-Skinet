using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);
        Task<CustomerBasket> UpdateBastekAsync(CustomerBasket basket);
        Task<bool> DeleteBaskAsync(string basketId);
    }
}