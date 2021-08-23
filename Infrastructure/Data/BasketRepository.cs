using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            // In this way we get the database available to use. Very Simple.
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBaskAsync(string basketId)
        {
            //The KeyDelete return true if the Id was deleted.
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            //Baskets are going to be stored in string in our Redis Database.
            //the Json that comes from our client will be serialized and stored in our Redis as string.
            var data = await _database.StringGetAsync(basketId);

            // If we have data we will deserialize it into our CustomerBasket passing the data as argument.
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBastekAsync(CustomerBasket basket)
        {
            //In this particular example we are storing the data in memory for 30 days.
            // We just set a serialized string into our Redis database.
            var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}