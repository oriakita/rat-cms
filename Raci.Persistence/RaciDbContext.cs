namespace Raci.Persistence
{
    using Audit.EntityFramework;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Raci.Domain.AccountAggregate;
    using Raci.Domain.CartAggregate;
    using Raci.Domain.ItemAggregate;
    using Raci.Domain.OrderAggregate;
    using Raci.Domain.RaciAccountAggregate;
    using Raci.Domain.SecurityAggregate;
    using Raci.Domain.ShopAggregate;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class RaciDbContext : AuditDbContext
    {
        public RaciDbContext()
        {

        }

        public RaciDbContext(DbContextOptions<RaciDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<RaciAccount> RaciAccounts { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }

        #region Security
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<UserAssignment> UserAssignments { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<Action> Actions { get; set; } 
        #endregion

        public static void UpdateDatabase(RaciDbContext context)
        {
            context.Database.Migrate();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaciDbContext).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        private new int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private new int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
