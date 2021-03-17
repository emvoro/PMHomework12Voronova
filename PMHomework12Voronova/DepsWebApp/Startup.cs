using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DepsWebApp.Clients;
using DepsWebApp.Options;
using DepsWebApp.Services;
using DepsWebApp.Authentication;
using System;
using System.IO;

namespace DepsWebApp
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
            // Add options
            services
                .Configure<CacheOptions>(Configuration.GetSection("Cache"))
                .Configure<NbuClientOptions>(Configuration.GetSection("Client"))
                .Configure<RatesOptions>(Configuration.GetSection("Rates"));

            // Add application services
            services.AddScoped<IRatesService, RatesService>();
            services.AddScoped<IAuthService, AuthService>();

            // Add authentication
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

            // Add NbuClient as Transient
            services.AddHttpClient<IRatesProviderClient, NbuClient>()
                .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(10));

            // Add CacheHostedService as Singleton
            services.AddHostedService<CacheHostedService>();


            // Add batch of Swashbuckle Swagger services
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Base64",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Scheme = "Base64",
                        Name = "Basic Authorization",
                        Description = "Base64 Code",
                        BearerFormat = "SessionId"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Id = "Session",
                                        Type = ReferenceType.SecurityScheme
                                    },
                                },
                                new string[0]
                            }
                    });

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication Demo API by Emilia Voronova", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "DepsWebApp.xml");
                if (File.Exists(filePath))
                {
                    options.IncludeXmlComments(filePath);
                }
            });

            // Add batch of framework services
            services.AddMemoryCache();
            services.AddControllers();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DI Demo App API v1"));
            }
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
