namespace ManagementPortalWPF.Services
{
    public interface INavigationService
    {
        void ShowEnter();
        void ShowMain();
        void ShowClient(int clientId);
        void CloseClient(int clientId, bool closed = false);
        void UpdateMain();
    }
}
