namespace Secrets.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AccessorModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        public Guid Key { get; set; }
    }
}
