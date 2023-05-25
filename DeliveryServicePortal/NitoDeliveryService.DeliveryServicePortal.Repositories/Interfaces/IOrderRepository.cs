using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order, int>
    {
        Task<Order> ReadWithIncludes(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUser(int userId, bool onlyActiveOrders = true);
        Task<IEnumerable<Order>> GetOrdersByPlace(int clientId, int placeId, bool onlyActiveOrders = true);
    }
}
