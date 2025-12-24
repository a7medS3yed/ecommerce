using System.ComponentModel.DataAnnotations;

namespace Admin.Dashbord.Models.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(256, ErrorMessage = "Role name size can't be more that 256 chars.")]
        public string Name { get; set; } = default!;
    }
}
