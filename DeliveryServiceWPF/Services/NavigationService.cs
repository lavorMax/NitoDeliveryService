using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.View;
using DeliveryServiceWPF.ViewModel;
using System.Collections.Generic;

namespace DeliveryServiceWPF.Services
{
    public class NavigationService : INavigationService
    {
        private Dictionary<int, OrderWindow> _orderWindows = new();
        private Dictionary<(int, int), PlaceWindow> _placeWindows = new();

        public MainWindow MainWindow;
        public EnterWindow EnterWindow;
        public RegistrationWindow RegistrationWindow;

        public IDeliveryServiceHttpClient _client;

        public MainViewModel MainViewModel;
        public EnterViewModel EnterViewModel;
        public RegistrationViewModel RegistrationViewModel;


        public void CloseOrder(int orderId, bool closed = false)
        {
            if (_orderWindows.TryGetValue(orderId, out var currentOrderWindow))
            {
                _orderWindows.Remove(orderId);
                if (!closed)
                {
                    currentOrderWindow.Close();
                }
            }
        }

        public void ClosePlace(int placeId, int clientId, bool closed = false)
        {
            if (_placeWindows.TryGetValue((placeId, clientId), out var currentPlaceWindow))
            {
                _placeWindows.Remove((placeId, clientId));
                if (!closed)
                {
                    currentPlaceWindow.Close();
                }
            }
        }

        public void ShowEnter()
        {
            EnterWindow.Show();
        }

        public void ShowMain(int userId)
        {
            MainViewModel.Initialize(userId);
            
            EnterWindow.Close();
            RegistrationWindow.Close();

            MainWindow.Show();
        }

        public void ShowPlace(int placeId, int clientId, int userId, int placeViewId, string address)
        {
            var placeVM = new PlaceViewModel(this, _client, placeId, clientId, userId, placeViewId, address);

            var currentOrderWindow = new PlaceWindow()
            {
                DataContext = placeVM
            };

            _placeWindows.Add((placeId, clientId), currentOrderWindow);
            currentOrderWindow.Show();
        }

        public void ShowRegister()
        {
            if (!RegistrationWindow.ShowActivated)
            {
                RegistrationWindow.Show();
            }
        }

        public void ShowOrder(int orderId)
        {
            var orderVM = new OrderViewModel(orderId, _client, this);

            var currentOrderWindow = new OrderWindow()
            {
                DataContext = orderVM
            };

            _orderWindows.Add(orderId, currentOrderWindow);
            currentOrderWindow.Show();
        }
    }
}
