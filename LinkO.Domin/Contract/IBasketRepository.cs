using LinkO.Domin.Models.BasketModule;
using System;
using System.Threading.Tasks;

namespace LinkO.Domin.Contract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketByUserEmailAsync(string userEmail);
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default);
        Task<bool> DeleteBasketAsync(string basketId);

        // New repository operations
        Task<CustomerBasket?> AddItemToBasketAsync(string userEmail, BasketItem item, TimeSpan timeToLive = default);
        Task<CustomerBasket?> UpdateItemQuantityAsync(string userEmail, int itemId, int quantity, TimeSpan timeToLive = default);
        Task<bool> RemoveItemAsync(string userEmail, int itemId);
        Task<bool> ClearBasketAsync(string userEmail);
    }
}
