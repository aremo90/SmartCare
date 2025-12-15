using LinkO.Domin.Contract;
using LinkO.Domin.Models.BasketModule;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LinkO.Persistence.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connection;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _database = connection.GetDatabase();
        }

        private static string UserIndexKey(string email) => $"userbasket:{email.Trim().ToLowerInvariant()}";
        
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            if (basket is null) return null;

            // Ensure no zero-quantity items
            basket.Items = (basket.Items ?? new List<BasketItem>()).Where(i => i.Quantity > 0).ToList();

            var jsonBasket = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            if (!isCreatedOrUpdated)
                return null;

            // Maintain email -> basketId index
            if (!string.IsNullOrWhiteSpace(basket.UserEmail))
            {
                var userKey = UserIndexKey(basket.UserEmail);
                await _database.StringSetAsync(userKey, basket.Id);
            }

            var saved = await _database.StringGetAsync(basket.Id);
            return JsonSerializer.Deserialize<CustomerBasket>(saved!);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            // Try to load basket to clean index
            var value = await _database.StringGetAsync(basketId);
            if (!value.IsNullOrEmpty)
            {
                try
                {
                    var basket = JsonSerializer.Deserialize<CustomerBasket>(value!);
                    if (basket is not null && !string.IsNullOrWhiteSpace(basket.UserEmail))
                    {
                        var userKey = UserIndexKey(basket.UserEmail);
                        await _database.KeyDeleteAsync(userKey);
                    }
                }
                catch (JsonException) { }
            }

            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketByUserEmailAsync(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return null;

            // Try index first
            var userKey = UserIndexKey(userEmail);
            var basketId = await _database.StringGetAsync(userKey);

            if (!basketId.IsNullOrEmpty)
            {
                var basketVal = await _database.StringGetAsync(basketId.ToString());

                if (!basketVal.IsNullOrEmpty)
                    return JsonSerializer.Deserialize<CustomerBasket>(basketVal!);
            }

            // Fallback to scan (safe fallback if index missing)
            foreach (var endpoint in _connection.GetEndPoints())
            {
                var server = _connection.GetServer(endpoint);
                foreach (var key in server.Keys(database: _database.Database))
                {
                    var value = await _database.StringGetAsync(key);
                    if (value.IsNullOrEmpty) continue;

                    try
                    {
                        var basket = JsonSerializer.Deserialize<CustomerBasket>(value!);
                        if (basket is not null && string.Equals(basket.UserEmail, userEmail, StringComparison.OrdinalIgnoreCase))
                        {
                            // Recreate index for future fast lookups
                            await _database.StringSetAsync(userKey, basket.Id);
                            return basket;
                        }
                    }
                    catch (JsonException) { }
                }
            }

            return null;
        }

        public async Task<CustomerBasket?> AddItemToBasketAsync(string userEmail, BasketItem item, TimeSpan timeToLive = default)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || item is null || item.Quantity <= 0)
                return null;

            var basket = await GetBasketByUserEmailAsync(userEmail);
            if (basket is null)
            {
                basket = new CustomerBasket
                {
                    Id = Guid.NewGuid().ToString(),
                    UserEmail = userEmail,
                    Items = new List<BasketItem> { item }
                };
            }
            else
            {
                basket.Items = basket.Items ?? new List<BasketItem>();
                var existing = basket.Items.FirstOrDefault(x => x.Id == item.Id);
                if (existing != null)
                {
                    existing.Quantity += item.Quantity;
                    existing.Price = item.Price;
                    existing.ProductName = item.ProductName;
                    existing.PictureUrl = item.PictureUrl;
                }
                else
                {
                    basket.Items.Add(item);
                }

                basket.Items = basket.Items.Where(i => i.Quantity > 0).ToList();
            }

            return await CreateOrUpdateBasketAsync(basket, timeToLive);
        }

        public async Task<CustomerBasket?> UpdateItemQuantityAsync(string userEmail, int itemId, int quantity, TimeSpan timeToLive = default)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return null;

            var basket = await GetBasketByUserEmailAsync(userEmail);
            if (basket is null) return null;

            basket.Items = basket.Items ?? new List<BasketItem>();
            var existing = basket.Items.FirstOrDefault(x => x.Id == itemId);
            if (existing is null) return null;

            if (quantity <= 0)
            {
                basket.Items.Remove(existing);
            }
            else
            {
                existing.Quantity = quantity;
            }

            // If basket empty -> delete it entirely
            if (!basket.Items.Any())
            {
                await DeleteBasketAsync(basket.Id);
                return null;
            }

            return await CreateOrUpdateBasketAsync(basket, timeToLive);
        }

        public async Task<bool> RemoveItemAsync(string userEmail, int itemId)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return false;

            var basket = await GetBasketByUserEmailAsync(userEmail);
            if (basket is null) return false;

            basket.Items = basket.Items ?? new List<BasketItem>();
            var existing = basket.Items.FirstOrDefault(x => x.Id == itemId);
            if (existing is null) return false;

            basket.Items.Remove(existing);

            if (!basket.Items.Any())
            {
                // Remove entire basket and index
                return await DeleteBasketAsync(basket.Id);
            }

            var updated = await CreateOrUpdateBasketAsync(basket);
            return updated is not null;
        }

        public async Task<bool> ClearBasketAsync(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return false;

            var userKey = UserIndexKey(userEmail);
            // basketId is of type RedisValue
            var basketId = await _database.StringGetAsync(userKey);

            if (basketId.IsNullOrEmpty)
                return false;

            await _database.KeyDeleteAsync(userKey);

            // Fix: Cast basketId to string so it converts to RedisKey
            return await _database.KeyDeleteAsync(basketId.ToString());
        }
    }
}
