using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.Generic;

namespace PlaceManagementPortalWPF.HttpClients
{
    public interface IPlaceManagementPortal
    {
        void SetupToken(string token);

        PlaceDTO GetPlaceByToken();

        IEnumerable<OrderDTO> GetAllOrders(int placeId);

        OrderDTO GetOrder(int orderId);
        void Prepare(int orderId);
        void Deliver(int orderId);
        void Close(int orderId);


        void DeleteDish(int dishId);
        void CreateDish(DishDTO dish);

        void DeletePaymentCofiguration(int configurationId);
        void CreatePaymentConfiguration(PaymentConfigDTO configuration);

        void UpdatePlace(PlaceDTO place);
    }
}
