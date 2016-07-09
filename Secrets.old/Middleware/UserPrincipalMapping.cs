namespace Secrets.Middleware
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Primitives;

    using Secrets.Entities;

    public class UserPrincipalMapping
    {
        public const string HeaderApiKey = "X-SECRETS-APIKEY";

        private readonly RequestDelegate next;

        public UserPrincipalMapping(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (var db = context.RequestServices.GetService<SecretContext>())
            {
                var accessor = GetAccessorByApi(db, context) ?? GetAccessorByLogin(db, context);

                if (accessor == null)
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                await this.next(context);
            }
        }

        private static Accessor GetAccessorByApi(SecretContext db, HttpContext context)
        {
            var accessor = default(Accessor);
            StringValues apikey;

            if (context.Request.Headers.TryGetValue(HeaderApiKey, out apikey))
            {
                accessor = db.Accessors.FirstOrDefault(a => a.Key == Guid.Parse(apikey[0]));

                if (accessor != null && context.User.Identity.Name != accessor.Login)
                {
                    context.User = new WindowsPrincipal(new WindowsIdentity(accessor.Login));
                }
            }

            return accessor;
        }

        private static Accessor GetAccessorByLogin(SecretContext db, HttpContext context)
        {
            var accessor = default(Accessor);
            var login = context.User.Identity.Name;

            if (string.IsNullOrWhiteSpace(login) == false)
            {
                accessor = db.Accessors.FirstOrDefault(a => a.Login == login);

                if (accessor != null && context.Request.Headers.ContainsKey(HeaderApiKey) == false)
                {
                    context.Request.Headers.Add(HeaderApiKey, accessor.Key.ToString());
                }
            }

            return accessor;
        }
    }
}
