using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.OrderDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Result = await _orderService.CreateOrderAsync(orderDTO, GetUserEmail());
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetAllOrders()
        {
            var Orders = await _orderService.GetAllOrdersAsync(GetUserEmail());
            return HandleResult(Orders);
        }

        // Use query parameter for email (emails contain characters that are awkward in route segments)
        [Authorize(Roles = "Admin")]
        [HttpGet("userHistory")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetUserHistoryOrders([FromQuery] string email)
        {
            var Orders = await _orderService.GetAllOrdersAsync(email);
            return HandleResult(Orders);
        }

        // Get UserOrder By Id (Admin) - use query parameters for both id and email
        [Authorize(Roles = "Admin")]
        [HttpGet("userOrder")]
        public async Task<ActionResult<OrderToReturnDTO>> GetUserOrder([FromQuery] Guid id, [FromQuery] string email)
        {
            var Order = await _orderService.GetOrderByIdAsync(id, email);
            return HandleResult(Order);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("OrdersInfo")]
        public async Task<ActionResult<TotalOrdersDTO>> GetOrdersInfo()
        {
            var OrdersInfo = await _orderService.GetTotalOrderInfo();
            return HandleResult(OrdersInfo);
        }

        // Get Order By Id (for authenticated user)
        [Authorize]
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid Id)
        {
            var Order = await _orderService.GetOrderByIdAsync(Id, GetUserEmail());
            return HandleResult(Order);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethod()
        {
            var Result = await _orderService.GetDeliveMethodsAsync();
            return HandleResult(Result);
        }
    }
}
