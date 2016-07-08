namespace Secrets.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Secrets.Entities;

    [Authorize]
    [Route("api/[controller]")]
    public class AccessorsController : Controller
    {
        private readonly SecretContext context;

        public AccessorsController(SecretContext context)
        {
            this.context = context;
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public bool Delete(Guid id)
        {
            var accessor = this.context.Accessors.FirstOrDefault(a => a.Key == id);

            if (accessor != null)
            {
                this.context.Accessors.Remove(accessor);
                return true;
            }

            return false;
        }

        [HttpGet]
        public List<Models.AccessorModel> Get()
        {
            return this.context.Accessors.Select(a => Mapper.Map<Models.AccessorModel>(a)).ToList();
        }

        [HttpPut]
        [Route("{email}")]
        public Guid Put(string email)
        {
            var accessor = this.context.Accessors.FirstOrDefault(a => a.Email == email);

            if (accessor == null)
            {
                accessor = new Accessor { Login = email, Email = email };
                this.context.Accessors.Add(accessor);
                this.context.SaveChanges(acceptAllChangesOnSuccess: true);
            }

            return accessor.Key;
        }
    }
}
