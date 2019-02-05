using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eventPublisher.data;
using eventPublisher.domain.contracts;
using eventPublisher.domain.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace eventPublisher.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IManageJwts, JwtManager>();
            services.AddTransient<IManageClaims, ClaimsManager>();
            services.AddTransient<IAuthorizeRequests, AuthorizationService>();
            services.AddTransient<IRepository, EventPublisherRepository>();
            services.AddDbContext<EventPublisherContext>(options => options.UseNpgsql("User ID=admin;Password=admin;Host=localhost;Port=5432;Database=EventPublisher"));
            services.AddTransient<IPublishEvents, EventPublisher>();
            
            services.AddMvc().AddXmlSerializerFormatters();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info { Title = "Event Publisher Service API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field. <br/>Example: Bearer yourJwt",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Publisher Service API V1");

            });

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
