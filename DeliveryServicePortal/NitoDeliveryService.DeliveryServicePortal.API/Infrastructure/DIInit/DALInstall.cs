using Microsoft.Extensions.DependencyInjection;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories;

namespace NitoDeliveryService.DeliveryServicePortal.API.Infrastructure.DIInit
{
    public static class DALInstall
    {
        public static void SetupDAL(this IServiceCollection services)
        {
            services.AddDbContext<DeliveryServiceDbContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPlaceViewRepository, PlaceViewRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IDishOrderRepository, DishOrderRepository>();
        }
    }
}
