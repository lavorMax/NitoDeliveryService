using NitoDeliveryService.Shared.View.Models.DeliveryServicePortal;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryServiceWPF.HttpClients
{
    public interface IDeliveryServiceHttpClient
    {
        void SetupToken(string token);
        Task<UserDto> GetUser(int userId); 
        Task<int> CreateUser(UserDto user);

        Task<List<PlaceDTO>> GetAllPlaces(string address);
        Task<PlaceDTO> GetPlace(int placeId, int clientId);

        Task<List<OrderDTO>> GetAllOrders();
        Task<OrderDTO> GetOrder(int orderId);
        Task CreateOrder(OrderDTO order);
        Task FinishOrder(int orderId);
    }
}
