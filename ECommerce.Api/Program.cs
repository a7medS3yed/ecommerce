
using System.Threading.Tasks;
using ECommerce.Api.CustomMiddleware;
using ECommerce.Api.Extentions;
using ECommerce.Api.Factories;
using ECommerce.Domain.Contracts;
using ECommerce.Persistance;
using ECommerce.Persistance.Data;
using ECommerce.Service;
using Microsoft.AspNetCore.Mvc;
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
            builder.Services.AddServiceLayer(builder.Configuration);
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });
            #endregion

            var app = builder.Build();

           await app.MigrateDatabase();
            await app.MigrateIdentityDatabase();

           await app.SeedData();
           await app.SeedIdentityData();

            #region Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandler>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync(); 
            #endregion
        }
    }
}
