using System.ComponentModel.DataAnnotations;

namespace Admin.Dashbord.Models.Roles
{
    public class UpdateRoleViewModel
    {
        public string Id { get; set; } = default!;
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(256, ErrorMessage = "Role name size can't be more that 256 chars.")]
        public string Name { get; set; } = default!;

        public bool IsSelected { get; set; }
    }
}
