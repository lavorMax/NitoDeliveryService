using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class DishOrderRepository : BaseRepository<DishOrder, int>, IDishOrderRepository
    {
        public DishOrderRepository(DeliveryServiceDbContext context) : base(context)
        {
        }
    }
}
