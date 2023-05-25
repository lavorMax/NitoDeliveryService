namespace PlaceManagementPortalWPF.Services
{
    public interface INavigationService
    {
        void ShowMain();
        void ShowEnter();
        void ShowOrder(int orderId);
        void CloseOrder(int orderId, bool closed = false);

        void ShowConfiguration();
    }
}
