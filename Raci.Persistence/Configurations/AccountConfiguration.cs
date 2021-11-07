using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raci.Domain.AccountAggregate;

namespace Raci.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts", "dbo");

            builder.HasKey(p => p.Id);

            builder.HasIndex(p => new { p.PhoneNumber, p.CountryCallingCode }).IsUnique();

            builder.Property(p => p.LanguageCode).HasDefaultValue("vi");

            builder.Property(p => p.Gender).HasEnumConversion();

            builder.Property(p => p.UserType).HasEnumConversion();

            builder.Property(p => p.AuditStatus).HasEnumConversion();
        }

    }
}
