using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.Dtos.Baskets
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue)]
        public int Quentity { get; set; }
    }
}