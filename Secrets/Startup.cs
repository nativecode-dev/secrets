namespace Secrets
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Secrets.Entities;
    using Secrets.Middleware;
    using Secrets.RouteConstraints;

    public class Startup
    {
        private static readonly Guid RootKey = new Guid("252C5B94-6614-4B2F-AEFC-37C4657CB487");

        public Startup(IHostingEnvironment env)
        {
            Mapper.Initialize(ConfigureMappings);

            this.Configuration =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                logger.AddConsole(this.Configuration.GetSection("Logging"));
                logger.AddDebug();
            }

            app.UseMiddleware<UserPrincipalMapping>();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();

            using (var db = app.ApplicationServices.GetService<SecretContext>())
            {
                if (db.Accessors.Any(a => a.Key == RootKey) == false)
                {
                    db.Accessors.Add(new Accessor { Email = "root@system.local", Key = RootKey, Login = @"domain.service@nativecode.local" });
                    db.SaveChanges(true);
                }
            }
        }

        private static void ConfigureMappings(IMapperConfigurationExpression config)
        {
            config.CreateMissingTypeMaps = true;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SecretContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("Secrets")));
            services.AddMvc();
            services.AddRouting(options => options.ConstraintMap.Add("email", typeof(EmailRouteConstraint)));
            services.AddSwaggerGen();
        }
    }
}
