using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using PlaceManagementPortalWPF.HttpClients;
using PlaceManagementPortalWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PlaceManagementPortalWPF.ViewModel
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IPlaceManagementPortal _managementClient;

        private ObservableCollection<DishOrderDTO> _items;
        public ObservableCollection<DishOrderDTO> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private DishOrderDTO _selectedItem;
        public DishOrderDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private int _orderId;
        private OrderDTO _orderDTO;

        private string _orderName;
        public string OrderName
        {
            get { return _orderName; }
            set
            {
                _orderName = value;
                OnPropertyChanged(nameof(OrderName));
            }
        }

        private string _orderStatus;
        public string OrderStatus
        {
            get { return _orderStatus; }
            set
            {
                _orderStatus = value;
                OnPropertyChanged(nameof(_orderStatus));
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand PrepareCommand { get; }
        public ICommand DeliverCommand { get; }

        public OrderViewModel(int orderId, IPlaceManagementPortal managementClient, INavigationService nav)
        {
            _managementClient = managementClient;
            _navigationService = nav;

            _orderId = orderId;
            ResetOrderDTO();

            CloseCommand = new Command(Close, CloseCanExecute);
            PrepareCommand = new Command(Prepare, PrepareCanExecute);
            DeliverCommand = new Command(Deliver, DeliverCanExecute);
        }

        private void Close(object parameter)
        {
            _managementClient.Close(_orderDTO.Id);
            ResetOrderDTO();
        }

        private void Prepare(object parameter)
        {
            _managementClient.Prepare(_orderDTO.Id);
            ResetOrderDTO();
        }

        private void Deliver(object parameter)
        {
            _managementClient.Deliver(_orderDTO.Id);
            ResetOrderDTO();
        }

        private bool CloseCanExecute(object parameter)
        {
            return _orderDTO.OrderStatus == OrderStatuses.Created;
        }

        private bool PrepareCanExecute(object parameter)
        {
            return _orderDTO.OrderStatus == OrderStatuses.Created;
        }

        private bool DeliverCanExecute(object parameter)
        {
            return _orderDTO.OrderStatus == OrderStatuses.Prepearing;
        }

        private void ResetOrderDTO()
        {
            _orderDTO = _managementClient.GetOrder(_orderId);
            OrderName = _orderDTO.ToString();
            OrderStatus = _orderDTO.OrderStatus.ToString();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
