using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.Services;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace DeliveryServiceWPF.ViewModel
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IDeliveryServiceHttpClient _client;

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

        private string _orderPrice;
        public string OrderPrice
        {
            get { return _orderPrice; }
            set
            {
                _orderPrice = value;
                OnPropertyChanged(nameof(OrderPrice));
            }
        }

        private string _orderDeliveryPrice;
        public string OrderDeliveryPrice
        {
            get { return _orderDeliveryPrice; }
            set
            {
                _orderDeliveryPrice = value;
                OnPropertyChanged(nameof(OrderDeliveryPrice));
            }
        }

        public ICommand FinishCommand { get; set; }
        public ICommand Closing { get; set; }

        public OrderViewModel(int orderId, IDeliveryServiceHttpClient managementClient, INavigationService nav)
        {
            _client = managementClient;
            _navigationService = nav;

            _orderId = orderId;
            ResetOrderDTO();

            FinishCommand = new Command(Close, CloseCanExecute);
            Closing = new Command(CloseWidow);
        }

        private void CloseWidow(object parameter)
        {
            _navigationService.CloseOrder(_orderId, true);
        }
        private void Close(object parameter)
        {
            _client.FinishOrder(_orderDTO.Id);
            ResetOrderDTO();
            _navigationService.CloseOrder(_orderId);
        }
        private bool CloseCanExecute(object parameter)
        {
            return _orderDTO.OrderStatus == OrderStatuses.Delivering;
        }

        private void ResetOrderDTO()
        {
            _orderDTO = _client.GetOrder(_orderId);
            OrderName = _orderDTO.ToString();
            OrderStatus = _orderDTO.OrderStatus.ToString();
            OrderPrice = _orderDTO.DishOrders.Sum(i => i.DishPrice * i.Number).ToString();
            OrderDeliveryPrice = _orderDTO.DeliveryPrice.ToString();

            Items = new ObservableCollection<DishOrderDTO>(_orderDTO.DishOrders);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
