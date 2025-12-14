using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction.Basket;
using ECommerce.Service.Abstraction.Caches;
using ECommerce.Service.Abstraction.Identity;
using ECommerce.Service.Abstraction.Orders;
using ECommerce.Service.Abstraction.Products;
using ECommerce.Service.Baskets;
using ECommerce.Service.Caches;
using ECommerce.Service.Identity;
using ECommerce.Service.Orders;
using ECommerce.Service.Products;
using ECommerce.Service.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Service
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Add AutoMapper Profiles
            services.AddAutoMapper(X => X.AddProfile(typeof(MappingProfile)));
            services.AddTransient<ProductPictureUrlResolver>();
            services.AddTransient<OrderPictureResolver>();


            // Add Service Layer Dependencies
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IOrderService, OrderService>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
               {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["JwtOptions:Issuer"],
                        ValidAudience = configuration["JwtOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!)
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
               });


            return services;
        }
    }
}
