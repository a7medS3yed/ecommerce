using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Persistance.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
           builder.Property(O => O.SubTotal)
                  .HasColumnType("decimal(8,2)");
           
           builder.OwnsOne(O => O.ShippingAddress, sa =>
           {
               sa.Property(s => s.FirstName)
                    .HasMaxLength(50);
               sa.Property(s => s.LastName)
                    .HasMaxLength(50);
                sa.Property(s => s.Street)
                    .HasMaxLength(50);
                sa.Property(s => s.City)
                    .HasMaxLength(50);
                sa.Property(s => s.Country)
                    .HasMaxLength(50);

           });
        }
    }
}
