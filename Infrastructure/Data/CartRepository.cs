using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase database;
        public CartRepository(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }

        public async Task<bool> DeleteCartAsync(string cartId)
        {
            return await database.KeyDeleteAsync(cartId);
        }

        public async Task<CustomerCart> GetCartAsync(string cartId)
        {
            var data = await database.StringGetAsync(cartId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(data);
        }

        public async Task<CustomerCart> UpdateCartAsync(CustomerCart cart)
        {
            var created = await database.StringSetAsync(
                cart.Id, 
                JsonSerializer.Serialize(cart),
                TimeSpan.FromDays(30));

            if(!created) return null;

            return await GetCartAsync(cart.Id);
        }
    }
}