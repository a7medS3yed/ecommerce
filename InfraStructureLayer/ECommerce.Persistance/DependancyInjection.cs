using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Persistance.Data;
using ECommerce.Persistance.Data.DataInitializer;
using ECommerce.Persistance.IdentityData.Contexts;
using ECommerce.Persistance.IdentityData.SeedData;
using ECommerce.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ECommerce.Persistance
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Here you can add your DbContext and other persistence-related services
           
            services.AddDbContext<StoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<StoreIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultIdentityConnection")));

            services.AddKeyedScoped<IDataInitializer, DataInitializer>("Default");
            services.AddKeyedScoped<IDataInitializer, IdentityDataIntiliazer>("Identity");
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddSingleton<IConnectionMultiplexer>(X =>
            {
               return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }

    }
}
