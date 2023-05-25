using ManagementPortalWPF.HttpClients;
using ManagementPortalWPF.View;
using ManagementPortalWPF.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace ManagementPortalWPF.Services
{
    public class NavigationService : INavigationService
    {
        private Dictionary<int, ClientWindow> _clientWindows = new();

        public MainWindow MainWindow;
        public EnterWindow EnterWindow;

        public IManagementPortalClient ManagementClient;

        public MainViewModel MainVM;

        public void ShowClient(int clientId)
        {
            var clientViewModel = new ClientViewModel(ManagementClient, clientId, this);
            var currentClientWindow = new ClientWindow(this, clientId)
            {
                DataContext = clientViewModel
            };

            _clientWindows.Add(clientId, currentClientWindow);
            currentClientWindow.Show();
        }

        public void CloseClient(int clientId, bool closed = false)
        {
            if(_clientWindows.TryGetValue(clientId, out var currentClientWindow))
            {
                _clientWindows.Remove(clientId);
                if (!closed)
                {
                    currentClientWindow.Close();
                }
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

        public void UpdateMain()
        {
            MainVM.Initialize();
        }
    }
}
