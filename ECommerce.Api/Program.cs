
using System.Threading.Tasks;
using ECommerce.Api.Extentions;
using ECommerce.Domain.Contracts;
using ECommerce.Persistance;
using ECommerce.Persistance.Data;
using ECommerce.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace ECommerce.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            #region services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddPersistanceServices(builder.Configuration);
            builder.Services.AddServiceLayer();
            #endregion

            var app = builder.Build();

           await app.MigrateDatabase();  

           await app.SeedData();

            #region Configure the HTTP request pipeline.


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync(); 
            #endregion
        }
    }
}
