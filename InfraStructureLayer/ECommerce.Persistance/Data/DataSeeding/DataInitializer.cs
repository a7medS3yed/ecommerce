using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance.Data.DataInitializer
{
    public class DataInitializer(StoreDbContext dbContext) : IDataInitializer
    {
        public async Task InitializeAsync()
        {
            var hasProducts = await dbContext.Products.AnyAsync();
            var hasProductBrands = await dbContext.ProductBrands.AnyAsync();
            var hasProductTypes = await dbContext.ProductTypes.AnyAsync();
            var hasDeliveryMethods = await dbContext.Set<DeliveryMethod>().AnyAsync();

            if (hasProducts && hasProductBrands && hasProductTypes && hasDeliveryMethods)
                return;

            try
            {
                if (!hasProductBrands)
                  await  SeedData<ProductBrand, int>("brands.json", dbContext.ProductBrands);

                if (!hasProductTypes)
                   await SeedData<ProductType, int>("types.json", dbContext.ProductTypes);

                 await dbContext.SaveChangesAsync();

                if (!hasProducts)
                   await SeedData<Product, int>("products.json", dbContext.Products);

                if(!hasDeliveryMethods)
                    await SeedData<DeliveryMethod, int>("delivery.json", dbContext.Set<DeliveryMethod>());

                await dbContext.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error Occer when initialize data, {ex}"); 
            }

        }

        private async Task SeedData<T, TKey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TKey>
        { 
            var filePath = @"..\..\ECommerce\InfraStructureLayer\ECommerce.Persistance\Data\DataSeeding\JsonFiles\" + fileName;

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file {fileName} was not found at path {filePath}.");
            try
            {

                var dataStream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data is not null)
                   await dbSet.AddRangeAsync(data);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error Occer when seed data, {ex}"); 
            }

        }
    }
}
