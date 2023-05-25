using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NitoDeliveryService.ManagementPortal.Services.HttpClients;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using NitoDeliveryService.ManagementPortal.Services.Services;

namespace NitoDeliveryService.ManagementPortal.API.Infrastructure.DIInit
{
    public static class BLInstall
    {
        public static void SetupBL(this IServiceCollection builder)
        {
            builder.AddAutoMapper(typeof(Mapper));

            builder.AddTransient<IPlaceManagementPortalHttpClient, PlaceManagementPortalHttpClient>();
            builder.AddTransient<IAuth0ApiClient, Auth0ApiClient>();
            builder.AddTransient<IClientDbService, ClientDbService>();

            builder.AddTransient<ISlotService, SlotService>();
            builder.AddTransient<IClientService, ClientService>();
        }
    }
}
