using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.ProductModule;

namespace ECommerce.Service.Specification.ProductsSpecification
{
    internal class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product, int>
    {
        public ProductsWithTypesAndBrandsSpecification() :base()
        {
            AddIncludeExpression(product => product.ProductType);
            AddIncludeExpression(product => product.ProductBrand);
        }
    }
}
