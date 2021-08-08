using System.Collections.Generic;

namespace Core.Models
{
    // Redis will create an instance of CustomerBasket, so we won't use EntityFramework for this purpose
    public class CustomerBasket
    {
        // An empty constructor to Redis create it without Id
        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        //Initialize it as an empty list of items.
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}