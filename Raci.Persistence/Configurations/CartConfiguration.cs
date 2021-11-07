using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raci.Domain.CartAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.ToTable("CartDetails", "dbo");

            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Item).WithMany(p => p.CartDetails).HasForeignKey(p => p.ItemId);

            builder.HasOne(p => p.Account).WithMany(p => p.CartDetails).HasForeignKey(p => p.AccountId);
        }

    }
}
