using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Persistance.IdentityData.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ECommerce.Persistance.IdentityData.SeedData
{
    internal class IdentityDataIntiliazer(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<StoreIdentityDbContext> logger
        )
        : IDataInitializer
    {
        public async Task InitializeAsync()
        {
            try
            {
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuberAdmin"));
                }

                if (!userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        DisplayName = "Ahmed Elsayed",
                        UserName = "ahemdelsayed",
                        Email = "ahemdelsayed@gmail.com",
                        PhoneNumber = "01013090596"
                    };

                    var user02 = new ApplicationUser()
                    {
                        DisplayName = "Body Adel",
                        UserName = "bodyadel",
                        Email = "bodyadelgmail.com",
                        PhoneNumber = "01013090595"
                    };

                    await userManager.CreateAsync(user01, "P@ssw0rd");
                    await userManager.CreateAsync(user02, "P@ssw0rd");

                    await userManager.AddToRoleAsync(user01, "SuberAdmin");
                    await userManager.AddToRoleAsync(user02, "Admin");
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured when seed data {ex.Message}");
            }
        }
    }
}
