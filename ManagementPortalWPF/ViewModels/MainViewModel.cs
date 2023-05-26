using ManagementPortalWPF.HttpClients;
using ManagementPortalWPF.Services;
using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.ManagementPortal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ManagementPortalWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IManagementPortalClient _managementClient;

        private string _clientNumber;
        private string _clientTitle;
        private ObservableCollection<ClientDto> _items;
        private ClientDto _selectedItem;


        public string ClientNumber
        {
            get { return _clientNumber; }
            set
            {
                _clientNumber = value;
                OnPropertyChanged(nameof(ClientNumber));
            }
        }

        public ClientDto SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _navigationService.ShowClient(value.Id);
            }
        }

        public string ClientTitle
        {
            get { return _clientTitle; }
            set
            {
               _clientTitle = value;
                OnPropertyChanged(nameof(ClientTitle));
            }
        }

        public ObservableCollection<ClientDto> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ICommand AddCommand { get; }

        public MainViewModel(INavigationService navService, IManagementPortalClient managementClient)
        {
            _navigationService = navService;
            _managementClient = managementClient;

            Items = new ObservableCollection<ClientDto>();

            AddCommand = new Command(Add, AddCanExecute);
        }

        public void Initialize()
        {
            var clientList = _managementClient.GetAll();
            Items = new ObservableCollection<ClientDto>(clientList);
        }

        private void Add(object parameter)
        {
            _managementClient.CreateCustomer(new ClientDto() { Title = ClientTitle, ResponsiblePhone = ClientNumber});
            ClientTitle = null;
            ClientNumber = null;

            _navigationService.UpdateMain();
        }

        private bool AddCanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_clientTitle) && !string.IsNullOrEmpty(_clientNumber);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
