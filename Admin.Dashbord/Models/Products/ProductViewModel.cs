using ECommerce.Domain.Entities.ProductModule;
using System.ComponentModel.DataAnnotations;

namespace Admin.Dashbord.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? PictureUrl { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product brand id is required")]
        public int BrandId { get; set; }
        public ProductBrand? Brand { get; set; }

        [Required(ErrorMessage = "Product type id is required")]
        public int TypeId { get; set; }
        public ProductType? Type { get; set; }
    }
}
