using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.Services;
using NitoDeliveryService.Shared.View;
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

        public void Initialize(int userId)
        {
            Places = new ObservableCollection<PlaceDTO>();
            _userId = userId;
        }

        private ObservableCollection<PlaceDTO> _places;
        private PlaceDTO _selectedPlace;
        private string _address;

        private ICommand _searchCommand;

        public ObservableCollection<PlaceDTO> Places
        {
            get { return _places; }
            set
            {
                _places = value;
                OnPropertyChanged();
            }
        }

        public PlaceDTO SelectedPlace
        {
            get { return _selectedPlace; }
            set
            {
                _selectedPlace = value;
                OnPropertyChanged();
                _navigationService.ShowPlace(_selectedPlace.Id, _selectedPlace.ClientId, _userId);
            }
        }

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

        private async void Search(object parameter)
        {
            var places = await _client.GetAllPlaces(Address);

            Places = new ObservableCollection<PlaceDTO>(places);
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
