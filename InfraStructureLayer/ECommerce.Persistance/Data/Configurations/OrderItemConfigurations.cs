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
    public class OrderItemConfigurations : IEntityTypeConfiguration<ItemOrder>
    {
        public void Configure(EntityTypeBuilder<ItemOrder> builder)
        {
            builder.Property(OI => OI.Price)
                   .HasColumnType("decimal(8,2)");

            builder.OwnsOne(OI => OI.Product, po =>
            {
                
                po.Property(p => p.ProductName)
                  .HasMaxLength(100);
                po.Property(p => p.PictureUrl)
                  .HasMaxLength(200);
            });
        }
    }
}
