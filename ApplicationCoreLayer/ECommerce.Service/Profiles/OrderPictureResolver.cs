using AutoMapper;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Shared.Dtos.Orders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Profiles
{
    public class OrderPictureResolver(IConfiguration configuration) : IValueResolver<ItemOrder, OrderItemDto, string>
    {
        public string Resolve(ItemOrder source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            // Implementation for resolving picture URL for OrderItemDto

            if (source.Product.PictureUrl is null) return string.Empty;

            // Here you can add logic to modify the URL if needed, e.g., adding a base URL
            if (source.Product.PictureUrl.StartsWith("http") || source.Product.PictureUrl.StartsWith("https"))
                return source.Product.PictureUrl;

            var baseUrl = configuration.GetSection("Urls")["BaseUrl"];

            return $"{baseUrl}{source.Product.PictureUrl}";

        }

    }
}
