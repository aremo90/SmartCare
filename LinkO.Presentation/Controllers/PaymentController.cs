using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.BasketDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }



        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreatePayment([FromQuery] int DeilveryMethod)
        {
            var Result = await paymentService.CreateOrUpdatePaymentIntnetAsync(GetUserEmail() , DeilveryMethod);
            return Ok(Result);
        }
    }
}
