using Microsoft.Extensions.DependencyInjection;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.ManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.ManagementPortal.Repositories.Repositories;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;

namespace NitoDelivery.ClientManager.API.Infrastructure.DIInit
{
    public static class DALInstall
    {
        public static void SetupDAL(this IServiceCollection builder)
        {
            builder.AddDbContext<ManagementPortalDbContext>();


            builder.AddTransient<IUnitOfWork, UnitOfWork>();

            builder.AddTransient<ISlotRepository, SlotRepository>();
            builder.AddTransient<IClientRepository, ClientRepository>();
        } 
    }
}
