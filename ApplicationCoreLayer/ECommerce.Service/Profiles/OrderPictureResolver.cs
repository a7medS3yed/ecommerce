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
    public class OrderPictureResolver
     : IValueResolver<ItemOrder, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(
            ItemOrder source,
            OrderItemDto destination,
            string destMember,
            ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product?.PictureUrl))
                return string.Empty;

            if (source.Product.PictureUrl.StartsWith("http"))
                return source.Product.PictureUrl;

            var baseUrl = _configuration["Urls:BaseUrl"];
            return $"{baseUrl}{source.Product.PictureUrl}";
        }
    }

}
