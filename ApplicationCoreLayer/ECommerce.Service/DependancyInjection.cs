using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction.Basket;
using ECommerce.Service.Abstraction.Caches;
using ECommerce.Service.Abstraction.Identity;
using ECommerce.Service.Abstraction.Products;
using ECommerce.Service.Baskets;
using ECommerce.Service.Caches;
using ECommerce.Service.Identity;
using ECommerce.Service.Products;
using ECommerce.Service.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Service
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            // Add AutoMapper Profiles
            services.AddAutoMapper(X => X.AddProfile(typeof(MappingProfile)));
            services.AddTransient<ProductPictureUrlResolver>();


            // Add Service Layer Dependencies
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IAuthentication, Authentication>();

           

            return services;
        }
    }
}
