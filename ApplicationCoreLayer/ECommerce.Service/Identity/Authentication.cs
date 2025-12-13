using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Service.Abstraction.Identity;
using ECommerce.Shared.CommenResponse;
using ECommerce.Shared.Dtos.Identitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Service.Identity
{
    internal class Authentication(UserManager<ApplicationUser> userManager, IConfiguration configuration) : IAuthentication
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user != null;
        }

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Error.NotFound("User.NotFound", $"User With This Email {email} is not found ");

            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateToken(user)
            };
        }

        public async Task<Result<UserDto>> LoginAsync(UserLoginDto loginDto)
        {
            var user  = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) 
                return Error.InvalidCredintals("Invalid email or password");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return Error.InvalidCredintals("Invalid email or password");

            var token = await GenerateToken(user);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = token
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
           
           var token = await GenerateToken(user);

            if (isCreated.Succeeded)
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Token = token
                };

            return isCreated.Errors.Select(E => Error.Validation( E.Code,E.Description)).ToList();
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var clamis = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach(var role in roles)
                clamis.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresDate = DateTime.Now.AddMinutes(
                Convert.ToDouble(configuration["JwtOptions:DurationInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JwtOptions:Issuer"],
                audience: configuration["JwtOptions:Audience"],
                claims: clamis,
                expires: expiresDate,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
