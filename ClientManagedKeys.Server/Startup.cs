using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using ClientManagedKeys.Server.Authentication;
using ClientManagedKeys.Server.Services;
using ClientManagedKeys.Server.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ClientManagedKeys.Server
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
            services.AddSingleton<IKeyService, KeyService>();
            services.AddSingleton<IKeyProvider, DemoKeyProvider>();
            
            services
                .AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverterWithAttributeSupport()));
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                    options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                })
                .AddApiKeySupport(options => {});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Client Managed Keys (v1)",
                    Version = "v1",
                    Description = Encoding.UTF8.GetString(typeof(Startup).Assembly.GetResourceAsBytes("SwaggerIntro.md"))
                });

                c.TagActionsBy(description => new List<string>()
                {
                    "Key operations"
                });

                c.SchemaFilter<DescribeEnumMemberValues>();
                
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    Type = SecuritySchemeType.ApiKey
                });

                var filePathServer = Path.Combine(AppContext.BaseDirectory, "ClientManagedKeys.Server.xml");
                var filePathModels = Path.Combine(AppContext.BaseDirectory, "ClientManagedKeys.Models.xml");
                
                c.IncludeXmlComments(filePathServer);
                c.IncludeXmlComments(filePathModels);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseReDoc(c =>
            {
                c.SpecUrl = "/swagger/v1/swagger.json";
                c.RoutePrefix = "docs";
            });
        }
    }
}