using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email);

        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveMethodsAsync();
        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string Email);
        Task<Result<TotalOrdersDTO>> GetTotalOrderInfo();
    }
}
