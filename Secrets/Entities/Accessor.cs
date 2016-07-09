namespace Secrets.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Accessor : Entity
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string Login { get; set; }

        public List<SecretAccessor> Secrets { get; set; } = new List<SecretAccessor>();
    }
}
