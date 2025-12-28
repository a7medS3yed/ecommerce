using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.Dtos.Products;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Service.Profiles
{
    public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(source.PictureUrl is null) return string.Empty;

            if(source.PictureUrl.StartsWith("http") || source.PictureUrl.StartsWith("https"))
                return source.PictureUrl;

            var baseUrl = configuration.GetSection("Urls")["BaseUrl"];

            return $"{baseUrl}{source.PictureUrl}";

        }
    }
}
