using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NitoDelivery.ClientManager.API.Infrastructure.DIInit;
using NitoDeliveryService.ManagementPortal.API.Infrastructure.DIInit;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;
using System.Text;

namespace ClientManager
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
            var auth0Options = Configuration.GetSection("Auth0ManagementPortalOptions").Get<Auth0ManagementPortalOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = auth0Options.Domain;
                options.Audience = auth0Options.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = auth0Options.Domain,
                    ValidAudience = auth0Options.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(auth0Options.ClientSecret))
                };
            });

            var managementDbOptions = Configuration.GetSection("ManagementPortalDbOptions").Get<ManagementPortalDbOptions>();
            services.AddSingleton(managementDbOptions);

            var placeManagementPortalOptions = Configuration.GetSection("PlaceManagementPortalOptions").Get<PlaceManagementPortalOptions>();
            services.AddSingleton(placeManagementPortalOptions);
            
            var auth0PlaceManagerOptions = Configuration.GetSection("Auth0PlaceManagerOptions").Get<Auth0PlaceManagementOptions>();
            services.AddSingleton(auth0PlaceManagerOptions);

            var placeDbOptions = Configuration.GetSection("PlaceDBServerOptions").Get<PlaceDBServerOptions>();
            services.AddSingleton(placeDbOptions);

            services.SetupDAL();
            services.SetupBL();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClientManager", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientManager v1"));
            }

            app.UseHttpsRedirection();

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
