using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Presentation.Attributes;
using ECommerce.Service.Abstraction.Products;
using ECommerce.Shared;
using ECommerce.Shared.Dtos.Products;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        
        [HttpGet]
        [RedisCahce]
        public async Task<ActionResult<PaginationResult<ProductDto>>> GetAllProducts(
           [FromQuery] ProductQueryParam queryParam)
        {
            var products = await productService.GetAllProducts(queryParam);
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductDto?>> GetProductById(int id)
        {
            var product = await productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await productService.GetAllBrands();
            return Ok(brands);
        }

        [HttpGet]
        [Route("types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var types = await productService.GetAllTypes();
            return Ok(types);
        }
    }
}
