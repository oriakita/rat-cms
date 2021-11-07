namespace Raci.Persistence.AuditNet
{
    using Audit.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Raci.Domain.AuditLog;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AuditNetConfiguration : IEntityTypeConfiguration<AuditLog>
    {

        public static void ConfigureWithSingleTable(string connectionString)
        {
            var ignoreEntities = new List<string>(new string[]
            {
                "EditableFieldArrayEnum",
                "EditableFieldEnum",
                "EditableFieldBool",
                "EditableFieldDateTime",

                "TargetAccount",
                "MarketplaceRecentSearch",
                "PropertyRequestImage",
                "StatisticsProperty",
            });

            Configuration.Setup()
                  .UseEntityFramework(_ => _
                  .UseDbContext(builder =>
                  {
                      var ef = (Audit.EntityFramework.AuditEventEntityFramework)builder;

                      switch (ef.EventType)
                      {
                          case nameof(RaciDbContext):
                              return new RaciDbContext(new DbContextOptionsBuilder<RaciDbContext>()
                                                                    .UseSqlServer(connectionString).Options);
                          //case nameof(HipsDbContext):
                          //    return new HipsDbContext(new DbContextOptionsBuilder<HipsDbContext>()
                          //                                          .UseSqlServer(connectionString).Options);
                          default:
                              throw new NotImplementedException($"Audit Net : we have not handled {ef.EventType} DbContext yet!");
                      }

                  })
                  .AuditTypeMapper(t => typeof(AuditLog))
                  .AuditEntityAction<AuditLog>((ev, entry, entity) =>
                  {
                      if (ignoreEntities.Contains(entry.EntityType.Name))
                          return false;

                      entity.AuditId = Guid.NewGuid();

                      entity.TablePk = entry.PrimaryKey.First().Value.ToString();

                      entity.EntityType = entry.EntityType.Name;

                      entity.AuditData = entry.ToJson();

                      entity.AuditDate = DateTime.UtcNow;

                      entity.AuditUser = Environment.UserName;

                      ev.CustomFields.TryGetValue("UserId", out var userId);
                      entity.AuditUser = userId?.ToString();

                      return true;
                  })
              .IgnoreMatchedProperties(true));
        }

        //
        public static void ConfigureWithSeparatingTables()
        {
            //Audit.Core.Configuration.Setup()
            //  .UseEntityFramework(_ => _
            //    .UseDbContext<ApplicationContext>()
            //    //.AuditTypeNameMapper(typeName => "Audit_" + typeName)
            //    .AuditTypeExplicitMapper(m => m
            //        .Map<Domain.LandlordAggregate.Landlord, AuditLandlord>()
            //        .Map<Domain.PropertyRequestAggregate.PropertyRequest, AuditPropertyRequest>()
            //        .Map<Domain.PropertyRequestAggregate.PropertyRequestImage, AuditPropertyRequestImage>()
            //      .AuditEntityAction<IBaseAudit>((ev, ent, auditEntity) =>
            //      {
            //          var entityFrameworkEvent = ev.GetEntityFrameworkEvent();

            //          auditEntity.AuditId = Guid.NewGuid();

            //          auditEntity.AuditAction = ent.Action;
            //          auditEntity.AuditDate = DateTime.UtcNow;
            //          auditEntity.AuditUser = ev.Environment.UserName;

            //          auditEntity.TransactionId = entityFrameworkEvent.TransactionId;

            //      })
            //      )
            //    );
        }

        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs", "dbo");

            builder.HasKey(p => p.AuditId);
        }
    }
}
