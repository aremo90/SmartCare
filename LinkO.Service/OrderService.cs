using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.BasketModule;
using LinkO.Domin.Models.Enum;
using LinkO.Domin.Models.OrderModule;
using LinkO.Service.Specification.OrderSpec;
using LinkO.ServiceAbstraction;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.OrderDTOS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkO.Service
{
    public class OrderService : IOrderService
    {
        #region CLR
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, ILogger<OrderService> logger , IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _basketRepository = basketRepository;
            _logger = logger;
            _paymentService = paymentService;
        }
        #endregion

        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email)
        {
            // Step 1 :-
            // Map From AddressDTO to Address
            //var orderAddress = new OrderAddress()
            //{
            //    FullName = orderDTO.Address.FullName,
            //    UserAddress = orderDTO.Address.UserAddress,
            //    PhoneNumber = orderDTO.Address.PhoneNumber,
            //    PaymentMethod = orderDTO.Address.PaymentMethod.ToString() == "Visa" ? PaymentMethod.Visa : PaymentMethod.cash
            //};
            var orderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            // Step 2 :-
            // Retrieves Basket and check items in it
            var Basket = await _basketRepository.GetBasketByUserEmailAsync(email);
            if (Basket is null)
                return Error.NotFound("No Basket Found For this User");


            //ArgumentNullException.ThrowIfNullOrEmpty(Basket.PaymentIntentId);

            //var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            //var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            //var ExistOrder = await OrderRepo.GetByIdAsync(Spec);
            //if (ExistOrder is not null) OrderRepo.Delete(ExistOrder);

            // Build order items
            List<OrderItem> OrderItems = new List<OrderItem>();
            foreach (var item in Basket.Items)
            {
                var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (Product is null)
                    return Error.NotFound("Product Not Found");
                OrderItems.Add(CreateOrderItem(item, Product));
            }

            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (DeliveryMethod is null)
                return Error.NotFound("DeliveryMethod not Found");

            var SubTotal = OrderItems.Sum(I => I.Price * I.Quantity);

            var Order = new Order()
            {
                Address = orderAddress,
                DeliveryMethod = DeliveryMethod,
                Items = OrderItems,
                SubTotal = SubTotal,
                UserEmail = email,
                PaymentIntentId = Basket.PaymentIntentId,
                
            };

            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            int Result = await _unitOfWork.SaveChangesAsync();
            if (Result == 0)
                return Error.Failure("Error Completing Order Please Try Again Later");

            await _paymentService.CreateOrUpdatePaymentIntnetAsync(email , DeliveryMethod.Id);
            await _basketRepository.DeleteBasketAsync(Basket.Id);
            return _mapper.Map<OrderToReturnDTO>(Order);
        }

        private static OrderItem CreateOrderItem(BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOredered()
                {
                    ProductId = Product.Id,
                    ProductName = Product.Name,
                    PictureUrl = Product.ImageUrl,
                },
                Price = Product.Price,
                Quantity = item.Quantity
            };
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email)
        {
            var Spec = new OrderSpecification(email);
            var Orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(Spec);

            if (Orders is null)
                return Error.NotFound("Order Not found");

            var Data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(Orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(Data);
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveMethodsAsync()
        {
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!DeliveryMethod.Any())
                return Error.NotFound("No Delivery Method Found");
            var Data = _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(DeliveryMethod);
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(Data);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string Email)
        {
            var Spec = new OrderSpecification(id, Email);
            var Order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(Spec);
            if (Order is null)
                return Error.NotFound("Order Not Found");
            return _mapper.Map<OrderToReturnDTO>(Order);
        }
    }
}
