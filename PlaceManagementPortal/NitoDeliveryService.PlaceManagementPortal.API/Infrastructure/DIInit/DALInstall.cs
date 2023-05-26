using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Repositories;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories;

namespace NitoDeliveryService.PlaceManagementPortal.API.Infrastructure.DIInit
{
    public static class DALInstall
    {
        public static void SetupDAL(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITokenParser, TokenParser>();

            services.AddScoped<IOverridingDbContextFactory<PlaceManagementDbContext>, PlaceManagementDbContextFactory>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IAuth0Client, Auth0Client>();

            services.AddTransient<IDishRepository, DishRepository>();
            services.AddTransient<IPlaceRepository, PlaceRepository>();
            services.AddTransient<IPaymentConfigurationRepository, PaymentConfigurationRepository>();
        }
        
    }
}
