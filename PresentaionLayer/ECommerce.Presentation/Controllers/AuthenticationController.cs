using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction.Identity;
using ECommerce.Shared.Dtos.Identitys;
using ECommerce.Shared.Dtos.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    public class AuthenticationController(IAuthentication authentication) : ApiBaseController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto registerDto)
        {
            var result = await authentication.RegisterAsync(registerDto);

            return HandleProblem(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto loginDto)
        {
            var result = await authentication.LoginAsync(loginDto);
            return HandleProblem(result);
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await authentication.CheckEmailAsync(email);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = User.FindFirstValue(ClaimTypes.Email);

            var result = await authentication.GetCurrentUserAsync(user!);

            return HandleProblem(result);

        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<ShippingAddressDto>> GetUserAddress() 
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            var result = await authentication.GetUserAddressAsync(user!);
            return HandleProblem(result);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<ShippingAddressDto>> UpdateUserAddress(ShippingAddressDto address)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            var result = await authentication.UpdateUserAddressAsync(user!, address);
            return HandleProblem(result);
        }
    }
}
