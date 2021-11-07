using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raci.Domain.OrderAggregate;

namespace Raci.Persistence.Configurations
{
    public class OrderConfiguration :
        IEntityTypeConfiguration<Order>,
        IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "dbo");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.OrderStatus).HasEnumConversion();

            builder.HasOne(p => p.Account).WithMany(p => p.Orders).HasForeignKey(p => p.AccountGuid);

            builder.HasOne(p => p.Shop).WithMany(p => p.Orders).HasForeignKey(p => p.ShopGuid);

            builder.HasOne(p => p.RaciAccount).WithMany(p => p.Orders).HasForeignKey(p => p.RaciAccountGuid);

            builder.HasMany(p => p.OrderDetails).WithOne(pf => pf.Order).HasForeignKey(pf => pf.OrderGuid);
        }

        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails", "dbo");

            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Order).WithMany(p => p.OrderDetails).HasForeignKey(p => p.OrderGuid);

            builder.HasOne(p => p.Item).WithMany(p => p.OrderDetails).HasForeignKey(p => p.ItemGuid);
        }
    }
}
