using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Services;
using NitoDeliveryService.PlaceManagementPortal.Services.HttpClients;
using NitoDeliveryService.DeliveryServicePortal.Services.Interfaces;
using NitoDeliveryService.DeliveryServicePortal.Services.HttpClients;

namespace NitoDeliveryService.DeliveryServicePortal.API.Infrastructure.DIInit
{
    public static class BLInstall
    {
        public static void SetupBL(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mapper));

            services.AddTransient<IAuth0Client, Auth0Client>();
            services.AddTransient<IPlaceManagementPortalHttpClient, PlaceManagementPortalHttpClient>();
            services.AddTransient<IPlaceViewService, PlaceViewService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOrderService, OrderService>();
        }
    }
}
