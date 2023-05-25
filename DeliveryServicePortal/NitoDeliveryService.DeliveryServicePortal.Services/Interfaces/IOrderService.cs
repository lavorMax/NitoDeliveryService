using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrder(OrderDTO order);
        Task<OrderDTO> GetOrder(int orderId);
        Task UpdateOrderStatus(int orderId, OrderStatuses status);
        Task<IEnumerable<OrderDTO>> GetOrdersByUser(int userId, bool onlyActiveOrders = true);
        Task<IEnumerable<OrderDTO>> GetOrdersByPlace(int clientId, int placeId, bool onlyActiveOrders = true);
    }
}
