using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.BasketDTOS;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IBasketService
    {
        Task<Result<BasketDTO>> CreateOrUpdateAsync(BasketDTO basket);
        Task<bool> DeleteBasketAsync(string id);

        // New service operations mapped to controller endpoints
        Task<Result<BasketDTO>> GetBasketByEmailAsync(string email);
        Task<Result<BasketDTO>> AddProductToCartAsync(string email, BasketItemDTO item);
        Task<Result<BasketDTO>> UpdateCartProductQuantityAsync(string email, int itemId, int quantity);
        Task<Result<bool>> RemoveCartItemAsync(string email, int itemId);
        Task<Result<bool>> ClearUserCartAsync(string email);
    }
}
