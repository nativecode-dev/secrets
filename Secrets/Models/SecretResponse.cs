namespace Secrets.Models
{
    using System;

    public class SecretResponse : Response
    {
        public SecretResponse(bool success = true)
            : base(success)
        {
        }

        public Guid Key { get; set; }

        public string Secret { get; set; }
    }
}
