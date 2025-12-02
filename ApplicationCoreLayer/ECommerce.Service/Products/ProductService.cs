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
using ECommerce.Shared;
using ECommerce.Shared.Dtos.Products;
using Microsoft.VisualBasic;

namespace ECommerce.Service.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrands()
        {
            var brands = await unitOfWork.GenaricRepository<ProductBrand, int>().GetAllAsync();
            return mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<PaginationResult<ProductDto>> GetAllProducts(ProductQueryParam queryParam)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(queryParam);

            var products = await unitOfWork.GenaricRepository<Product, int>().GetAllAsync(spec);

            var productToReturn =  mapper.Map<IEnumerable<ProductDto>>(products);

            var countSpec = new ProductWithCountSpecification(queryParam);

            var totalItems = await unitOfWork.GenaricRepository<Product, int>().CountAsync(countSpec);

            var countToReturn = productToReturn.Count();
            return new PaginationResult<ProductDto>
            {
                PageIndex = queryParam.PageIndex,
                PageSize = countToReturn,
                TotalItems = totalItems,
                Data = productToReturn
            };
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
