using LinkO.Domin.Contract;
using LinkO.Domin.Models.BasketModule;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.BasketDTOS;
using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Linq;
using LinkO.Shared.CommonResult;

namespace LinkO.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<Result<BasketDTO>> CreateOrUpdateAsync(BasketDTO basket)
        {
            var CustomerBasket = _mapper.Map<CustomerBasket>(basket);

            var CreatedOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);

            return _mapper.Map<BasketDTO>(CreatedOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }

        public async Task<Result<BasketDTO>> GetBasketByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Error.Unauthorized("You Are Not Authorized To Access This Basket");

            var basket = await _basketRepository.GetBasketByUserEmailAsync(email);
            if (basket is null)
                return Error.NotFound($"Basket for {email} not found.");

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task<Result<BasketDTO>> AddProductToCartAsync(string email, BasketItemDTO itemDto)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Error.Unauthorized("You Are Not Authorized To Add To Basket");

            var item = _mapper.Map<BasketItem>(itemDto);
            var basket = await _basketRepository.AddItemToBasketAsync(email, item);
            if (basket is null)
                return Error.NotFound("Could not add item to basket.");

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task<Result<BasketDTO>> UpdateCartProductQuantityAsync(string email, int itemId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Error.Unauthorized("You Are Not Authorized To Update Basket");

            var basket = await _basketRepository.UpdateItemQuantityAsync(email, itemId, quantity);
            if (basket is null)
                return Error.NotFound("Basket or item not found.");

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task<Result<bool>> RemoveCartItemAsync(string email, int itemId)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Error.Unauthorized("You Are Not Authorized To Modify Basket");

            var ok = await _basketRepository.RemoveItemAsync(email, itemId);
            return ok;
        }

        public async Task<Result<bool>> ClearUserCartAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Error.Unauthorized("You Are Not Authorized To Clear Basket");

            var ok = await _basketRepository.ClearBasketAsync(email);
            return ok;
        }
    }
}
