using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance.Data.DataInitializer
{
    public class DataInitializer(StoreDbContext dbContext) : IDataInitializer
    {
        public void Initialize()
        {
            var hasProducts = dbContext.Products.Any();
            var hasProductBrands = dbContext.ProductBrands.Any();
            var hasProductTypes = dbContext.ProductTypes.Any();

            if (hasProducts && hasProductBrands && hasProductTypes)
                return;

            try
            {
                if (!hasProductBrands)
                    SeedData<ProductBrand, int>("brands.json", dbContext.ProductBrands);

                if (!hasProductTypes)
                    SeedData<ProductType, int>("types.json", dbContext.ProductTypes);

                dbContext.SaveChanges();

                if (!hasProducts)
                {
                    SeedData<Product, int>("products.json", dbContext.Products);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error Occer when initialize data, {ex}"); 
            }

        }

        private void SeedData<T, TKey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TKey>
        { 
            var filePath = @"..\..\ECommerce\InfraStructureLayer\ECommerce.Persistance\Data\DataSeeding\JsonFiles\" + fileName;

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file {fileName} was not found at path {filePath}.");
            try
            {

                var dataStream = File.OpenRead(filePath);
                var data = JsonSerializer.Deserialize<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data is not null)
                    dbSet.AddRange(data);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error Occer when seed data, {ex}"); 
            }

        }
    }
}
