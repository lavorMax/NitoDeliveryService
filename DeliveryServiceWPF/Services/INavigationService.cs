namespace DeliveryServiceWPF.Services
{
    public interface INavigationService
    {
        void ShowMain(int userId);
        void ShowEnter();
        void ShowRegister();
        void ShowOrder(int orderId);
        void CloseOrder(int orderId, bool closed = false);

        void ShowPlace(int placeId, int clientId, int userId, int placeViewId, string address);
        void ClosePlace(int placeId, int clientId, bool closed = false);
    }
}
