using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction.Basket;
using ECommerce.Shared.Dtos.Baskets;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IBasketService basketService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasketItemsAsync(string basketId)
        {
            var result = await basketService.GetBasketByIdAsync(basketId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basketCreateOrUpdateDto)
        {
            var result = await basketService.CreateOrUpdateBasketAsync(basketCreateOrUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync([FromRoute] string id)
        {
            var result = await basketService.DeleteBasketAsync(id);
            return Ok(result);
        }
    }
}
