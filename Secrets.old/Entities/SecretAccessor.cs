namespace Secrets.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class SecretAccessor : Entity
    {
        [Required]
        public Accessor Accessor { get; set; }

        [Required]
        public Secret Secret { get; set; }
    }
}
