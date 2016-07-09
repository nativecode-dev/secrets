namespace Secrets
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using Secrets.Entities;

    public class SecretContext : DbContext
    {
        private readonly List<Action<EntityEntry>> handlers = new List<Action<EntityEntry>>(100);

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretContext"/> class.
        /// </summary>
        public SecretContext(DbContextOptions options)
            : base(options)
        {
            this.handlers.Add(HandleAuditProperties);
        }

        private static void HandleAuditProperties(EntityEntry entry)
        {
            var entity = entry.Entity as Entity;

            if (entity != null && entry.State == EntityState.Modified)
            {
                entity.DateModified = DateTimeOffset.UtcNow;
            }
        }

        public DbSet<Accessor> Accessors { get; protected set; }

        public DbSet<Secret> Secrets { get; protected set; }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.UpdateEntries();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessor>().HasIndex(accessor => accessor.Email).IsUnique();
            modelBuilder.Entity<Accessor>().HasIndex(accessor => accessor.Login).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        private void UpdateEntries()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                foreach (var handler in this.handlers)
                {
                    handler(entry);
                }
            }
        }
    }
}
