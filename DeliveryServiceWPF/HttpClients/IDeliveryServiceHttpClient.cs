using NitoDeliveryService.Shared.View.Models.DeliveryServicePortal;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.Generic;

namespace DeliveryServiceWPF.HttpClients
{
    public interface IDeliveryServiceHttpClient
    {
        void SetupToken(string token);
        UserDto GetUser(string login); 
        int CreateUser(UserDto user);

        List<PlaceViewDTO> GetAllPlaces(string address);
        PlaceDTO GetPlace(int placeId, int clientId);

        List<OrderDTO> GetAllOrders(int userId);
        OrderDTO GetOrder(int orderId);
        void CreateOrder(OrderDTO order);
        void FinishOrder(int orderId);
    }
}
