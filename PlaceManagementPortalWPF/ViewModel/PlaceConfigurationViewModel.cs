using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using PlaceManagementPortalWPF.HttpClients;
using PlaceManagementPortalWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace PlaceManagementPortalWPF.ViewModel
{
    public class PlaceConfigurationViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IPlaceManagementPortal _managementClient;

        private PlaceDTO _placeDTO;

        private string _placeName;
        private string _address;
        private string _placeDescription;

        private string _maxRange;
        private string _deliveryPrice;

        private string _dishName;
        private string _dishDescription;
        private string _dishPrice;

        private PaymentConfigDTO _selectedDeliveryConfiguration;
        private DishDTO _selectedMenuItem;
        private ObservableCollection<PaymentConfigDTO> _deliveryConfigurations;
        private ObservableCollection<DishDTO> _menuItems;

        

        public string PlaceName
        {
            get { return _placeName; }
            set
            {
                _placeName = value;
                OnPropertyChanged(nameof(PlaceName));
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string PlaceDescription
        {
            get { return _placeDescription; }
            set
            {
                _placeDescription = value;
                OnPropertyChanged(nameof(PlaceDescription));
            }
        }

        public string MaxRange
        {
            get { return _maxRange; }
            set
            {
                _maxRange = value;
                OnPropertyChanged(nameof(MaxRange));
            }
        }

        public string DeliveryPrice
        {
            get { return _deliveryPrice; }
            set
            {
                _deliveryPrice = value;
                OnPropertyChanged(nameof(DeliveryPrice));
            }
        }

        public string DishName
        {
            get { return _dishName; }
            set
            {
                _dishName = value;
                OnPropertyChanged(nameof(DishName));
            }
        }

        public string DishDescription
        {
            get { return _dishDescription; }
            set
            {
                _dishDescription = value;
                OnPropertyChanged(nameof(DishDescription));
            }
        }

        public string DishPrice
        {
            get { return _dishPrice; }
            set
            {
                _dishPrice = value;
                OnPropertyChanged(nameof(DishPrice));
            }
        }

        public ObservableCollection<PaymentConfigDTO> DeliveryConfigurations
        {
            get { return _deliveryConfigurations; }
            set
            {
                _deliveryConfigurations = value;
                OnPropertyChanged(nameof(DeliveryConfigurations));
            }
        }

        public ObservableCollection<DishDTO> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }

        public PaymentConfigDTO SelectedDeliveryConfiguration
        {
            get { return _selectedDeliveryConfiguration; }
            set
            {
                _selectedDeliveryConfiguration = value;
                OnPropertyChanged(nameof(SelectedDeliveryConfiguration));
            }
        }

        public DishDTO SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                _selectedMenuItem = value;
                OnPropertyChanged(nameof(SelectedMenuItem));
            }
        }

        public ICommand UpdateCommand { get; }
        public ICommand RemoveDeliveryConfigurationCommand { get; }
        public ICommand AddDeliveryConfigurationCommand { get; }
        public ICommand RemoveMenuItemCommand { get; }
        public ICommand AddMenuItemCommand { get; }

        public PlaceConfigurationViewModel(IPlaceManagementPortal managementClient, INavigationService nav)
        {
            _managementClient = managementClient;
            _navigationService = nav;


            UpdateCommand = new Command(Update, UpdateCanExecute);
            RemoveDeliveryConfigurationCommand = new Command(RemoveDeliveryConfiguration, RemoveDeliveryConfigurationCanExecute);
            AddDeliveryConfigurationCommand = new Command(AddDeliveryConfiguration, AddDeliveryConfigurationCanExecute);
            RemoveMenuItemCommand = new Command(RemoveMenuItem, RemoveMenuItemCanExecute);
            AddMenuItemCommand = new Command(AddMenuItem, AddMenuItemCanExecute);
        }


        private void Update(object parameter)
        {
            var place = new PlaceDTO()
            {
                Id = _placeDTO.Id,
                ClientId = _placeDTO.ClientId,
                Address = Address,
                Description = PlaceDescription,
                Name = _placeDTO.Name,
                Dishes = _placeDTO.Dishes,
                PaymentConfigurations = _placeDTO.PaymentConfigurations
            };

            _managementClient.UpdatePlace(place);
            ResetPlace();
        }

        private void RemoveDeliveryConfiguration(object parameter)
        {
            _managementClient.DeletePaymentCofiguration(_selectedDeliveryConfiguration.Id);
            ResetPlace();
        }

        private void AddDeliveryConfiguration(object parameter)
        {
            var deliveryPrice = decimal.Parse(DeliveryPrice);
            var maxRage = int.Parse(MaxRange);

            var configuration = new PaymentConfigDTO()
            {
                PlaceId = _placeDTO.Id,
                Price = deliveryPrice,
                MaxRange = maxRage
            };

            DeliveryPrice = null;
            MaxRange = null;
            _managementClient.CreatePaymentConfiguration(configuration);
            ResetPlace();
        }

        private void RemoveMenuItem(object parameter)
        {
            _managementClient.DeleteDish(_selectedMenuItem.Id);
            ResetPlace();
        }

        private void AddMenuItem(object parameter)
        {
            var price = decimal.Parse(DishPrice);

            var dish = new DishDTO()
            {
                PlaceId = _placeDTO.Id,
                Price = price,
                Name = DishName,
                Description = DishDescription
            };

            DishPrice = null;
            DishName = null;
            DishDescription = null;
            _managementClient.CreateDish(dish);
            ResetPlace();
        }

        private bool UpdateCanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(PlaceDescription);
        }

        private bool RemoveDeliveryConfigurationCanExecute(object parameter)
        {
            return SelectedDeliveryConfiguration != null;
        }

        private bool AddDeliveryConfigurationCanExecute(object parameter)
        {
            var resultMaxRange = int.TryParse(MaxRange, out var maxRange);
            var resultDeliveryPrice = decimal.TryParse(DeliveryPrice, out var deliveryPrice);

            return resultMaxRange && resultDeliveryPrice && maxRange > 0 && deliveryPrice > 0;
        }

        private bool RemoveMenuItemCanExecute(object parameter)
        {
            return SelectedMenuItem != null;
        }

        private bool AddMenuItemCanExecute(object parameter)
        {
            var menuItemPriceResult = decimal.TryParse(DishPrice, out var menuItemPrice);

            return menuItemPriceResult 
                && menuItemPrice > 0
                && !string.IsNullOrEmpty(DishName) 
                && !string.IsNullOrEmpty(DishDescription);
        }

        public void ResetPlace()
        {
            var place = _managementClient.GetPlaceByToken();

            PlaceName = place.Name;
            PlaceDescription = place.Description;
            Address = place.Address;
            _placeDTO = place;


            DeliveryConfigurations = new ObservableCollection<PaymentConfigDTO>(_placeDTO.PaymentConfigurations.OrderBy(i => i.MaxRange));
            MenuItems = new ObservableCollection<DishDTO>(_placeDTO.Dishes);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
