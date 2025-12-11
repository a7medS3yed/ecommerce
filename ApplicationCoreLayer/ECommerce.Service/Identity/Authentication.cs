using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Service.Abstraction.Identity;
using ECommerce.Shared.CommenResponse;
using ECommerce.Shared.Dtos.Identitys;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Service.Identity
{
    internal class Authentication(UserManager<ApplicationUser> userManager) : IAuthentication
    {
        public async Task<Result<UserDto>> LoginAsync(UserLoginDto loginDto)
        {
            var user  = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) 
                return Error.InvalidCredintals("Invalid email or password");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return Error.InvalidCredintals("Invalid email or password");

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = "Fake-JWT-Token"
            };
        }

        public async Task<Result<UserDto>> RegisterAsync(UserRegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };

           var isCreated = await userManager.CreateAsync(user, registerDto.Password);

            if (isCreated.Succeeded)
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Token = "Fake-JWT-Token"
                };

            return isCreated.Errors.Select(E => Error.Validation( E.Code,E.Description)).ToList();
        }
    }
}
