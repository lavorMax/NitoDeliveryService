using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NitoDeliveryService.DeliveryServicePortal.API.Infrastructure;
using NitoDeliveryService.DeliveryServicePortal.API.Infrastructure.DIInit;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Services.HttpClients;
using System.Text;

namespace DeliveryServicePortal
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
            var auth0Options = Configuration.GetSection("Auth0Options").Get<Auth0Options>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://" + auth0Options.Domain;
                    options.Audience = auth0Options.Audience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://" + auth0Options.Domain,
                        ValidAudience = auth0Options.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(auth0Options.ClientSecret))
                    };
                });



            services.AddAuthentication("ClientCredentials")
                .AddJwtBearer("ClientCredentials", options =>
                {
                    options.Authority = "https://" + auth0Options.Domain;
                    options.Audience = auth0Options.Audience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://" + auth0Options.Domain,
                        ValidAudience = auth0Options.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(auth0Options.SigingSecret))

                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientCredentialsPolicy", policy =>
                    policy.RequireAuthenticatedUser());
            });

            var placeManagementPortalOptions = Configuration.GetSection("PlaceManagementPortalOptions").Get<PlaceManagementPortalOptions>();
            services.AddSingleton(placeManagementPortalOptions);

            var auth0PlaceManagerOptions = Configuration.GetSection("Auth0PlaceManagerOptions").Get<Auth0PlaceManagementOptions>();
            services.AddSingleton(auth0PlaceManagerOptions);

            var dbOptions = Configuration.GetSection("DeliveryServiceDbOptions").Get<DeliveryServiceDbOptions>();
            services.AddSingleton(dbOptions);

            services.SetupDAL();
            services.SetupBL();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeliveryServicePortal", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeliveryServicePortal v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
