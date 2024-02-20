using Castle.Core.Internal;
using EPRN.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Common.Data
{
    public class EPRNContext : DbContext
    {
        public EPRNContext()
        {
        }

        public EPRNContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        public virtual DbSet<WasteType> WasteType { get; set; }

        public virtual DbSet<WasteSubType> WasteSubType { get; set; }

        public virtual DbSet<WasteJourney> WasteJourney { get; set; }

        public virtual DbSet<PackagingRecoveryNote> PRN { get; set; }

        public virtual DbSet<PrnHistory> PRNHistory { get; set; }

        public virtual DbSet<AuditItem> AuditItem { get;set; }

        public virtual DbSet<AuditChange> AuditChange { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackagingRecoveryNote>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getdate()");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.DetectChanges();
            var changedEntities = this.ChangeTracker.Entries();
            var changesToRecord = new List<AuditItem>();

            foreach(var change in changedEntities)
            {
                var entity = change.Entity;

                // we don't want to log audit items!
                if (entity is AuditItem)
                    continue;

                var changeRecords = new List<AuditChange>();

                var originalValues = this.Entry(entity).OriginalValues;
                var currentValues = this.Entry(entity).CurrentValues;
                    
                foreach(var prop in originalValues.Properties)
                {
                    var originalValue = originalValues[prop.Name];
                    var currentValue = currentValues[prop.Name];

                    if (change.State == EntityState.Added)
                    {
                        originalValue = null;
                        // if the property is currently the primary key, we'll forcibly set this to null, as 
                        currentValue = currentValues.Properties.FirstOrDefault(p => p.Name == prop.Name).IsKey() ? null : currentValue;
                    }

                    if (!Equals(originalValue, currentValue))
                    {
                        changeRecords.Add(new AuditChange
                        {
                            Field = prop.Name,
                            OriginalValue = originalValue?.ToString(),
                            NewValue = currentValue?.ToString(),
                        });
                    }
                }

                if (changeRecords.Any())
                {
                    var changeItem = new AuditItem
                    {
                        Action = change.State.ToString(),
                        EntityId = change.State != EntityState.Added ? GetKey(change.Entity) : null,
                        Table = entity.GetType().Name,
                        Username = "USERNAME",
                        Timestamp = DateTime.UtcNow,
                        Changes = changeRecords
                    };

                    changesToRecord.Add(changeItem);
                }
            }

            await AuditItem.AddRangeAsync(changesToRecord);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public virtual int? GetKey(object entity)
        {
            var type = entity.GetType();
            var properties = type.GetProperties();

            foreach(var property in properties)
            {
                var attributes = property.GetAttributes<KeyAttribute>();

                foreach(var attribute in attributes)
                {
                    if (attribute is KeyAttribute)
                    {
                        var val = property.GetValue(entity);

                        if (val != null)
                        {
                            return (int)Convert.ChangeType(val, typeof(int));
                        }
                    }
                }
            }

            return null;
        }
    }
}
