using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.BasketModule;
using LinkO.Domin.Models.OrderModule;
using LinkO.Service.Exceptions;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.BasketDTOS;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;

        public PaymentService(IBasketRepository basketRepository, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this.configuration = configuration;
        }


        public async Task<BasketDTO> CreateOrUpdatePaymentIntnetAsync(string email ,int deilveryMethod)
        {
            StripeConfiguration.ApiKey = configuration["StripeKey:SecretKey"];


            var Basket = await _basketRepository.GetBasketByUserEmailAsync(email);
            if (Basket is null)
                throw new BasketNotFoundException(Basket!.Id);

            var Product = _unitOfWork.GetRepository<Domin.Models.Product, int>();
            foreach (var Item in Basket.Items)
            {
                var product = await Product.GetByIdAsync(Item.Id) ?? throw new ProductNotFoundException(Item.Id);
                Item.Price = product.Price;
            }
            // DeliveryMethod
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(deilveryMethod);

            Basket.ShippingPrice = DeliveryMethod.Price;

            var BasketAmount = (long)(Basket.Items.Sum(item => item.Quantity * item.Price) + DeliveryMethod.Price) * 100;

            // Create PaymentIntent
            var PaymentService = new PaymentIntentService();
            if (Basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await PaymentService.CreateAsync(options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
                Basket.DeliveryMethodId = DeliveryMethod.Id;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = BasketAmount,
                };
                Basket.DeliveryMethodId = DeliveryMethod.Id;
                await PaymentService.UpdateAsync(Basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(Basket);
            // Mapping
            return _mapper.Map<CustomerBasket, BasketDTO>(Basket);

        }

    }
}
