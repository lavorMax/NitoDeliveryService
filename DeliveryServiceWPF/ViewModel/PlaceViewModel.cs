using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.Services;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DeliveryServiceWPF.ViewModel
{
    public class PlaceViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IDeliveryServiceHttpClient _client;
        private int _userId;
        private int _clientId;
        private int _placeId;
        private int _placeViewId;
        private string _address;

        public PlaceViewModel(INavigationService navigationService, IDeliveryServiceHttpClient managementClient, int placeId, int clientId, int userId, int placeViewId, string address)
        {
            _navigationService = navigationService;
            _client = managementClient;
            _userId = userId;
            _clientId = clientId;
            _placeId = placeId;
            _placeViewId = placeViewId;
            _address = address;

            AddCommand = new Command(Add, CanAdd);
            RemoveCommand = new Command(Remove, CanRemove);
            CreateOrderCommand = new Command(CreateOrder, CanCreateOrder);
            Closing = new Command(Close);
        }

        private string _placeName;
        private string _placeAddress;

        private ObservableCollection<DishDTO> _placeItems;
        private ObservableCollection<DishOrderDTO> _dishOrders;
        private DishDTO _selectedPlaceItem;
        private DishOrderDTO _selectedOrderItem;

        public ICommand AddCommand;
        public ICommand RemoveCommand;
        public ICommand CreateOrderCommand;
        public ICommand Closing;

        public string PlaceName
        {
            get { return _placeName; }
            set
            {
                _placeName = value;
                OnPropertyChanged(PlaceName);
            }
        }

        public string PlaceAddress
        {
            get { return _placeAddress; }
            set
            {
                _placeAddress = value;
                OnPropertyChanged(PlaceAddress);
            }
        }

        public ObservableCollection<DishDTO> PlaceItems
        {
            get { return _placeItems; }
            set
            {
                _placeItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DishOrderDTO> DishOrders
        {
            get { return _dishOrders; }
            set
            {
                _dishOrders = value;
                OnPropertyChanged();
            }
        }

        public DishDTO SelectedPlaceItem
        {
            get { return _selectedPlaceItem; }
            set
            {
                _selectedPlaceItem = value;
                OnPropertyChanged();
            }
        }

        public DishOrderDTO SelectedOrderItem
        {
            get { return _selectedOrderItem; }
            set
            {
                _selectedOrderItem = value;
                OnPropertyChanged();
            }
        }

        private void Close(object parameter)
        {
            _navigationService.ClosePlace(_placeId, _clientId, true);
        }

        private void Add(object parameter)
        {
            var result = DishOrders.FirstOrDefault(i => i.DishId == SelectedPlaceItem.Id);
            if (result == null)
            {
                result.Number = result.Number + 1;
            }
            else
            {
                var dishOrder = new DishOrderDTO()
                {
                    DishId = SelectedPlaceItem.Id,
                    Number = 1,
                    DishPrice = SelectedPlaceItem.Price,
                    DishName = SelectedPlaceItem.Name
                };
                DishOrders.Add(dishOrder);
            }
        }

        private bool CanAdd(object parameter)
        {
            return SelectedPlaceItem != null;
        }

        private void Remove(object parameter)
        {
            DishOrders.Remove(SelectedOrderItem);
        }

        private bool CanRemove(object parameter)
        {
            return SelectedOrderItem != null;
        }

        private void CreateOrder(object parameter)
        {
            var Order = new OrderDTO()
            {
                PlaceId = _placeId,
                ClientId = _clientId,
                UserId = _userId,
                OrderStatus = OrderStatuses.Created,
                Adress = _address,
                PlaceViewId = _placeViewId,

            };

            Order.DishOrders = new List<DishOrderDTO>();
            foreach(var item in DishOrders)
            {
                Order.DishOrders.Add(item);
            }

            _navigationService.ClosePlace(_placeId, _clientId);
        }

        private bool CanCreateOrder(object parameter)
        {
            return DishOrders != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
