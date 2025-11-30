using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Persistance.Data;
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

        public static async Task<WebApplication> SeedData(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
           
            var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            await dataInitializer.InitializeAsync();
            return app;
        }
    }
}
