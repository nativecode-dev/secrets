namespace Secrets.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Secret : Entity, IAccessor
    {
        /// <summary>
        /// Gets or sets the accessors for the secret.
        /// </summary>
        public List<SecretAccessor> Accessors { get; set; } = new List<SecretAccessor>();

        /// <summary>
        /// Gets or sets the API key for the secret.
        /// </summary>
        [StringLength(256)]
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the login for the secret.
        /// </summary>
        [StringLength(256)]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the maxmimum number of uses before the secret is disabled.
        /// </summary>
        public int? MaxUse { get; set; }

        /// <summary>
        /// Gets or sets the current counter for the secret.
        /// </summary>
        public int? MaxUseCounter { get; set; }

        /// <summary>
        /// Gets or sets the password for the secret.
        /// </summary>
        [StringLength(128)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the URL for the secret.
        /// </summary>
        [DataType(DataType.Url)]
        [Required]
        [StringLength(1024)]
        public string Url { get; set; }

        [StringLength(2048)]
        public string UrlPattern { get; set; }
    }
}
