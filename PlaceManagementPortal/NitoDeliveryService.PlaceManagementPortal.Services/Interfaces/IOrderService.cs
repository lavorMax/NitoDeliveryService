using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IOrderService
    {
        Task ChangeStatusToClosed(int orderId);
        Task ChangeStatusToPrepearing(int orderId);
        Task ChangeStatusToDelivering(int orderId);
        Task<IEnumerable<OrderDTO>> GetActiveOrders();
        Task<OrderDTO> GetOrder(int orderId);
    }
}
