using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using PlaceManagementPortalWPF.HttpClients;
using PlaceManagementPortalWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PlaceManagementPortalWPF.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IPlaceManagementPortal _managementClient;

        private PlaceDTO _placeDTO;
        private string _placeName;

        private ObservableCollection<OrderDTO> _orders;
        private OrderDTO _selectedOrder;

        public string PlaceName
        {
            get { return _placeName; }
            set
            {
                _placeName = value;
                OnPropertyChanged(nameof(PlaceName));
            }
        }

        public ObservableCollection<OrderDTO> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public OrderDTO SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _navigationService.ShowOrder(value.Id);
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        public ICommand OpenConfigCommand { get; }

        public MainViewModel(IPlaceManagementPortal managementClient, INavigationService nav)
        {
            _managementClient = managementClient;
            _navigationService = nav;

            OpenConfigCommand = new Command(OpenConfigPage);
        }

        public void Initialize()
        {
            var place = _managementClient.GetPlaceByToken();

            _placeName = place.Name;
            _placeDTO = place;

            var orders = _managementClient.GetAllOrders(_placeDTO.Id);
            Orders = new ObservableCollection<OrderDTO>(orders);
        }

        private void OpenConfigPage(object parameter)
        {
            _navigationService.ShowConfiguration();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
