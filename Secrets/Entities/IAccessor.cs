namespace Secrets.Entities
{
    using System.ComponentModel.DataAnnotations;

    public interface IAccessor
    {
        /// <summary>
        /// Gets the API key for the secret.
        /// </summary>
        [StringLength(256)]
        string ApiKey { get; }

        /// <summary>
        /// Gets the login for the secret.
        /// </summary>
        [StringLength(256)]
        string Login { get; }

        /// <summary>
        /// Gets the maxmimum number of uses before the secret is disabled.
        /// </summary>
        int? MaxUse { get; }

        /// <summary>
        /// Gets the current counter for the secret.
        /// </summary>
        int? MaxUseCounter { get; }

        /// <summary>
        /// Gets the password for the secret.
        /// </summary>
        [StringLength(128)]
        string Password { get; }

        /// <summary>
        /// Gets the URL for the secret.
        /// </summary>
        [DataType(DataType.Url)]
        [Required]
        [StringLength(1024)]
        string Url { get; }

        /// <summary>
        /// Gets the URL pattern for the secret.
        /// </summary>
        [StringLength(2048)]
        string UrlPattern { get; }
    }
}