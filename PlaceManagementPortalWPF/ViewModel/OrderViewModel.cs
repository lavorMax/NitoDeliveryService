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
        private string _userName;
        private string _userPhone;
        private string _userAddress;

        public string OrderName
        {
            get { return _orderName; }
            set
            {
                _orderName = value;
                OnPropertyChanged(nameof(OrderName));
            }
        }

        public string UserAddress
        {
            get { return _userAddress; }
            set
            {
                _userAddress = value;
                OnPropertyChanged(nameof(UserAddress));
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string UserPhone
        {
            get { return _userPhone; }
            set
            {
                _userPhone = value;
                OnPropertyChanged(nameof(UserPhone));
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

        public OrderViewModel(int orderId, IPlaceManagementPortal managementClient)
        {
            _managementClient = managementClient;

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
            UserName = $"{_orderDTO.User.Name} {_orderDTO.User.Surname}";
            UserPhone = _orderDTO.User.Phone;
            UserAddress = _orderDTO.Adress;
            Items = new ObservableCollection<DishOrderDTO>(_orderDTO.DishOrders);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
