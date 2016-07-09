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
        /// Initializes a new instance of the <see cref="SecretContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public SecretContext(DbContextOptions options)
            : base(options)
        {
            this.handlers.Add(HandleAuditProperties);
        }

        /// <summary>
        /// Gets or sets the accessors.
        /// </summary>
        public DbSet<Accessor> Accessors { get; protected set; }

        /// <summary>
        /// Gets or sets the secrets.
        /// </summary>
        public DbSet<Secret> Secrets { get; protected set; }

        /// <summary>
        /// save changes as an asynchronous operation.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        /// been sent successfully to the database.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.</returns>
        /// <remarks><para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </para></remarks>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.UpdateEntries();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.</remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessor>().HasIndex(accessor => accessor.Email).IsUnique();
            modelBuilder.Entity<Accessor>().HasIndex(accessor => accessor.Login).IsUnique();
            modelBuilder.Entity<Secret>().HasIndex(secret => secret.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        private static void HandleAuditProperties(EntityEntry entry)
        {
            var entity = entry.Entity as Entity;

            if (entity != null && entry.State == EntityState.Modified)
            {
                entity.DateModified = DateTimeOffset.UtcNow;
            }
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
