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
        public ProductsWithTypesAndBrandsSpecification() :base(null)
        {
            AddIncludeExpression(product => product.ProductType);
            AddIncludeExpression(product => product.ProductBrand);
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
             AddIncludeExpression(product => product.ProductType);
             AddIncludeExpression(product => product.ProductBrand);
        }
    }
}
