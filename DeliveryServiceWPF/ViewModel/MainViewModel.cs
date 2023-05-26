using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.Services;
using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.DeliveryServicePortal;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DeliveryServiceWPF.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IDeliveryServiceHttpClient _client;
        private int _userId;

        public MainViewModel(INavigationService navigationService, IDeliveryServiceHttpClient managementClient)
        {
            _navigationService = navigationService;
            _client = managementClient;

            SearchCommand = new Command(Search, CanSearch);
        }

        public void Initialize(int userId = 0)
        {
            if(_userId != 0)
            {
                userId = _userId;
            }

            var orders = _client.GetAllOrders(userId);
            
            if(orders != null)
            {
                Orders = new ObservableCollection<OrderDTO>(orders);
            }
            else
            {
                Orders = new ObservableCollection<OrderDTO>();
            }


            Places = new ObservableCollection<PlaceViewDTO>();
            _userId = userId;
        }

        private ObservableCollection<OrderDTO> _orders;
        private OrderDTO _selectedOrder;


        private ObservableCollection<PlaceViewDTO> _places;
        private PlaceViewDTO _selectedPlace;
        private string _address;

        private ICommand _searchCommand;

        public ObservableCollection<PlaceViewDTO> Places
        {
            get { return _places; }
            set
            {
                _places = value;
                OnPropertyChanged();
            }
        }

        public PlaceViewDTO SelectedPlace
        {
            get { return _selectedPlace; }
            set
            {
                _navigationService.ShowPlace(value.PlaceId, value.ClientId, _userId, value.Id, _lastAddress);
            }
        }

        public ObservableCollection<OrderDTO> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public OrderDTO SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
                if(value != null)
                {
                    _navigationService.ShowOrder(_selectedOrder.Id);
                }
            }
        }


        public string _lastAddress;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand
        {
            get { return _searchCommand; }
            set
            {
                _searchCommand = value;
                OnPropertyChanged();
            }
        }

        private void Search(object parameter)
        {
            _lastAddress = Address;

            var places = _client.GetAllPlaces(Address);

            Places = new ObservableCollection<PlaceViewDTO>(places);
        }

        private bool CanSearch(object parameter)
        {
            return !string.IsNullOrEmpty(Address);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
