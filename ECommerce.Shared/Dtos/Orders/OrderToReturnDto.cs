    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Dtos.Orders
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string PaymentIntentId { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
        public string OrderStatus { get; set; }
        public ShippingAddressDto ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
