using PlaceManagementPortalWPF.HttpClients;
using PlaceManagementPortalWPF.View;
using PlaceManagementPortalWPF.ViewModel;
using System.Collections.Generic;

namespace PlaceManagementPortalWPF.Services
{
    public class NavigationService : INavigationService
    {
        private Dictionary<int, OrderWindow> _orderWindows = new();

        public MainWindow MainWindow;
        public EnterWindow EnterWindow;
        public PlaceConfigurationWindow PlaceConfigurationWindow;

        public IPlaceManagementPortal ManagementClient;

        public MainViewModel MainVM;
        public PlaceConfigurationViewModel PlaceConfigurationVM;



        public void CloseOrder(int orderId, bool closed = false)
        {
            if (_orderWindows.TryGetValue(orderId, out var currentClientWindow))
            {
                _orderWindows.Remove(orderId);
                if (!closed)
                {
                    currentClientWindow.Close();
                }
            }
        }

        public void ShowConfiguration()
        {
            if(PlaceConfigurationWindow == null || !PlaceConfigurationWindow.IsVisible)
            {
                PlaceConfigurationWindow = new PlaceConfigurationWindow()
                {
                    DataContext = PlaceConfigurationVM
                };
                PlaceConfigurationWindow.Show();
                PlaceConfigurationVM.ResetPlace();
            }
        }

        public void ShowEnter()
        {
            EnterWindow.Show();
        }

        public void ShowMain()
        {
            MainVM.Initialize();
            EnterWindow.Close();
            MainWindow.Show();
        }

        public void ShowOrder(int orderId)
        {
            var clientViewModel = new OrderViewModel(orderId, ManagementClient);

            var currentOrderWindow = new OrderWindow(this, orderId)
            {
                DataContext = clientViewModel
            };

            _orderWindows.Add(orderId, currentOrderWindow);
            currentOrderWindow.Show();
        }
    }
}
