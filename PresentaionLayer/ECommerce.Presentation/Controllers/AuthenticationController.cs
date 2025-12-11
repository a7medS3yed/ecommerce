using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction.Identity;
using ECommerce.Shared.Dtos.Identitys;
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
    }
}
