using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.CommenResponse;
using ECommerce.Shared.Dtos.Identitys;

namespace ECommerce.Service.Abstraction.Identity
{
    public interface IAuthentication
    {
        Task<Result<UserDto>> RegisterAsync(UserRegisterDto registerDto);
        Task<Result<UserDto>> LoginAsync(UserLoginDto loginDto);
        Task<bool> CheckEmailAsync(string email);
        Task<Result<UserDto>> GetCurrentUserAsync(string email);
    }
}
