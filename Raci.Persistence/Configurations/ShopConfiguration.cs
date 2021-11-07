using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raci.Domain.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Persistence.Configurations
{
    public class ShopConfiguration :
        IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.ToTable("Shops", "dbo");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.AuditStatus).HasEnumConversion();

            builder.HasMany(p => p.Orders).WithOne(pf => pf.Shop).HasForeignKey(pf => pf.ShopGuid);

            builder.HasMany(p => p.Staffs).WithOne(pf => pf.Shop).HasForeignKey(pf => pf.ShopGuid);
        }
    }
}
