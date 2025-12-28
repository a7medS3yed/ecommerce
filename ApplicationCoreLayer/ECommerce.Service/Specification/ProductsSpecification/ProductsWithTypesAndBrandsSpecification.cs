using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;

namespace ECommerce.Service.Specification.ProductsSpecification
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product, int>
    {
       
        public ProductsWithTypesAndBrandsSpecification(ProductQueryParam queryParam)
            : base(P => (!queryParam.brandId.HasValue || P.BrandId == queryParam.brandId.Value)
            && (!queryParam.typeId.HasValue || P.TypeId == queryParam.typeId.Value)
            && (string.IsNullOrEmpty(queryParam.search) || P.Name.ToLower().Contains(queryParam.search.ToLower()))
            )
        {
            AddIncludeExpression(product => product.ProductType);
            AddIncludeExpression(product => product.ProductBrand);

            switch(queryParam.sort)
            {
                case ProductSortOption.NameAsc:
                    AddOrderBy(product => product.Name);
                    break;
                case ProductSortOption.NameDesc:
                    AddOrderByDesc(product => product.Name);
                    break;
                case ProductSortOption.PriceAsc:
                    AddOrderBy(product => product.Price);
                    break;
                case ProductSortOption.PriceDesc:
                    AddOrderByDesc(product => product.Price);
                    break;
                default:
                    AddOrderBy(product => product.Id);
                    break;
            }

            ApplyPaging(queryParam.PageSize, queryParam.PageIndex);
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
             AddIncludeExpression(product => product.ProductType);
             AddIncludeExpression(product => product.ProductBrand);
        }
    }
}
