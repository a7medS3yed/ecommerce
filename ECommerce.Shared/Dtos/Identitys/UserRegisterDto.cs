using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.Dtos.Identitys
{
    public class UserRegisterDto
    {
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        [Phone]
        public string PhoneNumber { get; set; } = default!;

    }
}
