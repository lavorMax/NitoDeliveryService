using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IDeliveryServiceHttpClient
    {
        Task ChangeStatus(int orderId, OrderStatuses status);
        Task<IEnumerable<OrderDTO>> GetOrders(int placeId, int clientId, bool onlyActive = true);
        Task<OrderDTO> GetOrder(int orderId);
        Task UpdatePlace(PlaceDTO place);
        Task CreatePlace(PlaceDTO place);
        Task DeletePlace(int placeId, int clientId);
    }
}
