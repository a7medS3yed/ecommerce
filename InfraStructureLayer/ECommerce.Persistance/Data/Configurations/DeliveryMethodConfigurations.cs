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
    public class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(D => D.Price)
                   .HasColumnType("decimal(8,2)");
            builder.Property(D => D.ShortName)
                   .HasMaxLength(50);
            builder.Property(D => D.Description)
                   .HasMaxLength(100);
            builder.Property(builder => builder.DeliveryTime)
                   .HasMaxLength(50);
        }
    }
}
