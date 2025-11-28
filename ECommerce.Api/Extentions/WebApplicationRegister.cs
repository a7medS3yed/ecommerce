using ECommerce.Domain.Contracts;
using ECommerce.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Extentions
{
    public static class WebApplicationRegister
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();

            return app;
        }

        public static WebApplication SeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
           
            var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            dataInitializer.Initialize();

            return app;
        }
    }
}
