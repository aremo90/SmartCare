using LinkO.Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkO.Shared.DTOS; // adjust if you have different DTO namespace

namespace LinkO.ServiceAbstraction
{
    public interface IPaymentService
    {
        Task<BasketDTO> CreateOrUpdatePaymentIntnetAsync(string email , int deilveryMethod);
    }
}
