namespace Secrets.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class Entity
    {
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset DateModified { get; set; } = DateTimeOffset.UtcNow;

        [Key]
        public Guid Key { get; set; } = Guid.NewGuid();
    }
}