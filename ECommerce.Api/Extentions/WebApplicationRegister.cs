using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Persistance.Data;
using ECommerce.Persistance.IdentityData.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Extentions
{
    public static class WebApplicationRegister
    {
        public static async Task<WebApplication> MigrateDatabase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
               await dbContext.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> MigrateIdentityDatabase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();

            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedData(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
           
            var dataInitializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Default");
            await dataInitializer.InitializeAsync();
            return app;
        }

        public static async Task<WebApplication> SeedIdentityData(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var dataInitializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Identity");
            await dataInitializer.InitializeAsync();
            return app;
        }
    }
}
