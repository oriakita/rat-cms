using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raci.Domain.ItemAggregate;

namespace Raci.Persistence.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items", "dbo");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Size).HasEnumConversion();

            builder.Property(p => p.AuditStatus).HasEnumConversion();

            builder.HasMany(p => p.OrderDetails).WithOne(p => p.Item).HasForeignKey(p => p.ItemGuid);
        }
    }
}
