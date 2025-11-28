
using ECommerce.Api.Extentions;
using ECommerce.Domain.Contracts;
using ECommerce.Persistance;
using ECommerce.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace ECommerce.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            #region services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddPersistanceServices(builder.Configuration);
            #endregion

            var app = builder.Build();

            app.MigrateDatabase();  

            app.SeedData();

            #region Configure the HTTP request pipeline.


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run(); 
            #endregion
        }
    }
}
