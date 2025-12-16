using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.BasketDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        // GET: /api/basket/me
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BasketDTO>> GetLoggedUserCart()
        {
            var email = GetUserEmail();
            var basket = await _basketService.GetBasketByEmailAsync(email);
            return HandleResult(basket);
        }

        // POST: /api/basket/items
        [HttpPost("items")]
        [Authorize]
        public async Task<ActionResult<BasketDTO>> AddProductToCart([FromBody] BasketItemDTO item)
        {

            var result = await _basketService.AddProductToCartAsync(GetUserEmail(), item);
            return HandleResult(result);
        }

        // PUT: /api/basket/items/{itemId}
        [HttpPut("items/{itemId}")]
        [Authorize]
        public async Task<ActionResult<BasketDTO>> UpdateCartProductQuantity(int itemId, [FromQuery] int quantity)
        {
            var result = await _basketService.UpdateCartProductQuantityAsync(GetUserEmail(), itemId, quantity);
            return HandleResult(result);
        }

        // DELETE: /api/basket/items/{itemId}
        [HttpDelete("items/{itemId}")]
        [Authorize]
        public async Task<ActionResult<bool>> RemoveSpecificCartItem(int itemId)
        {
            var result = await _basketService.RemoveCartItemAsync(GetUserEmail(), itemId);
            return HandleResult(result);
        }

        // DELETE: /api/basket/clear
        [HttpDelete("clear")]
        [Authorize]
        public async Task<ActionResult<bool>> ClearUserCart()
        {
            var result = await _basketService.ClearUserCartAsync(GetUserEmail());
            return HandleResult(result);
        }

        // Keep legacy create/update endpoint if needed
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basketDto)
        {
            var Basket = await _basketService.CreateOrUpdateAsync(basketDto);
            return HandleResult(Basket);
        }
    }
}
