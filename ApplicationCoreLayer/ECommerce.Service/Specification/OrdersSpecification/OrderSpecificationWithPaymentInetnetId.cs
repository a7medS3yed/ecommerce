using ECommerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specification.OrdersSpecification
{
    public class OrderSpecificationWithPaymentInetnetId : BaseSpecification<Order, Guid>
    {
        public OrderSpecificationWithPaymentInetnetId(string paymentIntentId)
            : base(X => X.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}

