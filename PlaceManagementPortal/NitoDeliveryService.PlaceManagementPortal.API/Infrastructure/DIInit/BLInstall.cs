using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NitoDeliveryService.PlaceManagementPortal.Services.HttpClients;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Services;
using PlaceManagementPortal;

namespace NitoDeliveryService.PlaceManagementPortal.API.Infrastructure.DIInit
{
    public static class BLInstall
    {
        public static void SetupBL(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mapper));

            services.AddTransient<IDeliveryServiceHttpClient, DeliveryServiceHttpClient>();

            services.AddTransient<IPlaceService, PlaceService>();
            services.AddTransient<IPaymentConfigurationService, PaymentConfigurationService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IDishService, DishService>();
        }
    }
}
