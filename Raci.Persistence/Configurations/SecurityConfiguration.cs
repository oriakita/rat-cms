using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raci.Domain.SecurityAggregate;

namespace Raci.Persistence.Configurations
{
    public class SecurityConfiguration :
        IEntityTypeConfiguration<Role>,
        IEntityTypeConfiguration<RolePermission>,
        IEntityTypeConfiguration<UserAssignment>,
        IEntityTypeConfiguration<Module>,
        IEntityTypeConfiguration<Function>,
        IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).HasEnumConversion();
            builder.HasMany(p => p.RolePermissions).WithOne(pf => pf.Role).HasForeignKey(pf => pf.RoleId);
            builder.HasMany(p => p.UserAssignments).WithOne(pf => pf.Role).HasForeignKey(pf => pf.RoleId);
        }

        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions", "dbo");
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Role).WithMany(p => p.RolePermissions).HasForeignKey(p => p.RoleId);
            builder.HasOne(p => p.Function).WithMany(p => p.RolePermissions).HasForeignKey(p => p.FunctionId);
        }

        public void Configure(EntityTypeBuilder<UserAssignment> builder)
        {
            builder.ToTable("UserAssignments", "dbo");
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Role).WithMany(p => p.UserAssignments).HasForeignKey(p => p.RoleId);
            builder.HasOne(p => p.RaciAccount).WithMany(p => p.UserAssignments).HasForeignKey(p => p.RaciAccountId);
        }

        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules", "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).HasEnumConversion();
            builder.HasMany(p => p.Functions).WithOne(pf => pf.Module).HasForeignKey(pf => pf.ModuleId);
        }

        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("Functions", "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).HasEnumConversion();
            builder.HasOne(p => p.Module).WithMany(p => p.Functions).HasForeignKey(p => p.ModuleId);
            builder.HasMany(p => p.Actions).WithOne(pf => pf.Function).HasForeignKey(pf => pf.FunctionId);
            builder.HasMany(p => p.RolePermissions).WithOne(pf => pf.Function).HasForeignKey(pf => pf.FunctionId);
        }

        public void Configure(EntityTypeBuilder<Action> builder)
        {
            builder.ToTable("Actions", "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).HasEnumConversion();
            builder.HasOne(p => p.Function).WithMany(p => p.Actions).HasForeignKey(p => p.FunctionId);
        }
    }
}
