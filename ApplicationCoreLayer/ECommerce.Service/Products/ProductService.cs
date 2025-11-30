using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Service.Abstraction.Products;
using ECommerce.Service.Specification.ProductsSpecification;
using ECommerce.Shared.Dtos.Products;

namespace ECommerce.Service.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrands()
        {
            var brands = await unitOfWork.GenaricRepository<ProductBrand, int>().GetAllAsync();
            return mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await unitOfWork.GenaricRepository<Product, int>().GetAllAsync(spec);

            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypes()
        {
            var types = await unitOfWork.GenaricRepository<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await unitOfWork.GenaricRepository<Product, int>().GetByIdAsync(spec);

            return mapper.Map<ProductDto>(product);
        }
    }
}
