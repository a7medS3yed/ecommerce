using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;

namespace ECommerce.Service.Specification.ProductsSpecification
{
    public class ProductWithCountSpecification : BaseSpecification<Product, int>
    {
        public ProductWithCountSpecification(ProductQueryParam queryParam)
           : base(P => (!queryParam.brandId.HasValue || P.BrandId == queryParam.brandId.Value)
            && (!queryParam.typeId.HasValue || P.TypeId == queryParam.typeId.Value)
            && (string.IsNullOrEmpty(queryParam.search) || P.Name.ToLower().Contains(queryParam.search.ToLower()))
            )
        {
        }
    }
}
