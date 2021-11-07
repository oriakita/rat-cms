using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raci.Domain.RaciAccountAggregate;

namespace Raci.Persistence.Configurations
{
    public class RaciAccountConfiguration : IEntityTypeConfiguration<RaciAccount>
    {
        public void Configure(EntityTypeBuilder<RaciAccount> builder)
        {
            builder.ToTable("RaciAccounts", "dbo");

            builder.HasKey(p => p.Id);

            builder.HasIndex(p => new { p.PhoneNumber }).IsUnique();

            builder.Property(p => p.LanguageCode).HasDefaultValue("vi");

            builder.Property(p => p.Gender).HasEnumConversion();

            builder.Property(p => p.Role).HasEnumConversion();

            builder.Property(p => p.AuditStatus).HasEnumConversion();

            builder.HasOne(p => p.Shop).WithMany(p => p.Staffs).HasForeignKey(p => p.ShopGuid);

            builder.HasMany(p => p.Orders).WithOne(pf => pf.RaciAccount).HasForeignKey(pf => pf.RaciAccountGuid);
        }
    }
}
