using ECommerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specification.OrdersSpecification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string email)
            : base(o => o.UserEmail == email)
        {
            AddIncludeExpression(o => o.Items);
            AddIncludeExpression(o => o.DeliveryMethod);
            AddOrderByDesc(o => o.OrderDate);
        }

        public OrderSpecification(Guid orderId, string email)
            : base(o => o.Id == orderId && o.UserEmail == email)
        {
            AddIncludeExpression(o => o.Items);
            AddIncludeExpression(o => o.DeliveryMethod);
        }
    }
}
