using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces
{
    public interface IDishOrderRepository : IBaseRepository<DishOrder, int>
    {
    }
}
