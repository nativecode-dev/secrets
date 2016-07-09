namespace Secrets.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Primitives;

    using Secrets.Entities;
    using Secrets.Middleware;
    using Secrets.Models;

    [Authorize]
    [Route("api/[controller]")]
    public class SecretsController : Controller
    {
        private readonly SecretContext context;

        public SecretsController(SecretContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<SecretModel> Get()
        {
            return this.GetAccessor().Secrets.Select(s => Mapper.Map<SecretModel>(s.Secret)).ToList();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public SecretModel Get(Guid id)
        {
            return Mapper.Map<SecretModel>(this.context.Secrets.FirstOrDefault(s => s.Key == id));
        }

        [HttpPut]
        public Guid Put([FromBody] SecretModel request)
        {
            var accessor = this.GetAccessor();
            var secret = this.context.Secrets.SingleOrDefault(s => s.Accessors.Any(a => a.Key == accessor.Key) && s.Url == request.Url);

            if (secret == null)
            {
                secret = Mapper.Map<Secret>(request);
                secret.Accessors.Add(new SecretAccessor { Accessor = accessor, Secret = secret });

                this.context.Secrets.Add(secret);
                this.context.SaveChanges(true);
            }

            return secret.Key;
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public bool Patch(Guid id, [FromBody] SecretModel request)
        {
            var secret = this.context.Secrets.SingleOrDefault(s => s.Key == id);

            if (secret != null)
            {
                secret.ApiKey = request.ApiKey;
                secret.Login = request.Login;
                secret.Password = request.Password;
                secret.Url = request.Url;
                this.context.SaveChanges(true);

                return true;
            }

            return false;
        }

        private Accessor GetAccessor()
        {
            return this.context.Accessors.Include(a => a.Secrets).ThenInclude(s => s.Secret).Single(a => a.Key == this.GetAccessorKey());
        }

        private Guid GetAccessorKey()
        {
            StringValues values;

            if (this.Request.Headers.TryGetValue(UserPrincipalMapping.HeaderApiKey, out values))
            {
                return Guid.Parse(values[0]);
            }

            throw new HttpRequestException("No API key was provided.");
        }
    }
}
