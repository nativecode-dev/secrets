namespace Secrets.Models
{
    using Secrets.Entities;

    public class SecretModel : IAccessor
    {
        public string ApiKey { get; set; }

        public string Login { get; set; }

        public int? MaxUse { get; set; }

        public int? MaxUseCounter { get; set; }

        public string Password { get; set; }

        public string Url { get; set; }

        public string UrlPattern { get; set; }
    }
}
