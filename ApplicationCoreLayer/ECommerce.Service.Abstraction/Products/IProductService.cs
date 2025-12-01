using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared;
using ECommerce.Shared.Dtos.Products;

namespace ECommerce.Service.Abstraction.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts(ProductQueryParam queryParam);
        Task<ProductDto?> GetProductById(int id);
        Task<IEnumerable<BrandDto>> GetAllBrands();
        Task<IEnumerable<TypeDto>> GetAllTypes();

    }
}
