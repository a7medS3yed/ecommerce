using ECommerce.Service.Abstraction.Payment;
using ECommerce.Shared.Dtos.Baskets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class PaymentController(IPaymentService paymentService) : ApiBaseController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await paymentService.CreateOrUpdatePaymentIntentAsync(basketId);

            return HandleProblem(result);
        }

    }
}
