using ManagementPortalWPF.HttpClients;
using ManagementPortalWPF.Services;
using NitoDeliveryService.Shared.Models.DTOs;
using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.ManagementPortal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ManagementPortalWPF.ViewModels
{   
    public class ClientViewModel
    {
        private readonly IManagementPortalClient _managementClient;
        private readonly INavigationService _navigationService;

        private int _clientId;
        private ClientDto _clientDto;
        private string _clientNumber;
        private string _clientTitle;
        
        private string _slotName;
        private string _managerEmail;
        private string _numberOfSlots;
        private ObservableCollection<SlotDto> _items;
        private SlotDto _selectedItem;

        public string ClientNumber
        {
            get { return _clientNumber; }
            set
            {
                _clientNumber = value;
                OnPropertyChanged(nameof(ClientNumber));
            }
        }

        public string SlotName
        {
            get { return _slotName; }
            set
            {
                _slotName = value;
                OnPropertyChanged(nameof(SlotName));
            }
        }

        public string ManagerEmail
        {
            get { return _managerEmail; }
            set
            {
                _managerEmail = value;
                OnPropertyChanged(nameof(ManagerEmail));
            }
        }

        public string NumberOfSlots
        {
            get { return _numberOfSlots; }
            set
            {
                _numberOfSlots = value;
                OnPropertyChanged(nameof(NumberOfSlots));
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

        public ObservableCollection<SlotDto> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public SlotDto SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand DeleteCommand { get; }
        public ICommand AddSlotsCommand { get; }

        public ICommand InitializeSlotCommand { get; }
        public ICommand DeinitializeSlotCommand { get; }
        public ICommand GetSlotCredentialsCommand { get; }
        public ICommand DeleteSlotCommand { get; }

        public ClientViewModel(IManagementPortalClient managementClient, int clientId, INavigationService nav)
        {
            _clientId = clientId;

            _managementClient = managementClient;
            _navigationService = nav;

            UpdatePage();

            ClientNumber = _clientDto.ResponsiblePhone;
            ClientTitle = _clientDto.Title;

            DeleteCommand = new Command(Delete);
            AddSlotsCommand = new Command(AddSlots, AddSlotsCanExecute);
            InitializeSlotCommand = new Command(InitializeSlotCommandAction, InitializeSlotCommandCanExecute);
            DeinitializeSlotCommand = new Command(DeinitializeSlotCommandAction, DeinitializeSlotCommandCanExecute);
            GetSlotCredentialsCommand = new Command(GetSlotCredentialsCommandAction, GetSlotCredentialsCommandCanExecute);
            DeleteSlotCommand = new Command(DeleteSlotCommandAction, DeleteSlotCommandCanExecute);
        }

        private void InitializeSlotCommandAction(object parameter)
        {
            var request = new InitializeSlotRequest()
            {
                SlotId = _selectedItem.Id,
                Name = SlotName,
                Email = ManagerEmail,
                ClientId = _clientId
            };

            _managementClient.InitSlot(request);

            SlotName = null;
            ManagerEmail = null;

            UpdatePage();
        }

        private void DeinitializeSlotCommandAction(object parameter)
        {
            _managementClient.DeinitSlot(_selectedItem.Id);

            UpdatePage();
        }

        private void GetSlotCredentialsCommandAction(object parameter)
        {
            var result = _managementClient.GetCreds(_selectedItem.Id);

            MessageBox.Show($"Login: {result.auth0login} & Password: {result.auth0password}");
        }

        private void DeleteSlotCommandAction(object parameter)
        {
            _managementClient.DeleteSlot(_selectedItem.Id);

            UpdatePage();
        }

        private bool InitializeSlotCommandCanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(SlotName) && _selectedItem != null && !_selectedItem.IsUsed;
        }

        private bool DeinitializeSlotCommandCanExecute(object parameter)
        {
            return _selectedItem != null && _selectedItem.IsUsed;
        }

        private bool GetSlotCredentialsCommandCanExecute(object parameter)
        {
            return _selectedItem!=null && _selectedItem.IsUsed;
        }

        private bool DeleteSlotCommandCanExecute(object parameter)
        {
            return _selectedItem != null;
        }

        private void Delete(object parameter)
        {
            _managementClient.RemoveCustomer(_clientId);
            _navigationService.UpdateMain();
            _navigationService.CloseClient(_clientId);
        }

        private void AddSlots(object parameter)
        {
            var numberOfSlots = int.Parse(NumberOfSlots);

            _managementClient.CreateSlot(numberOfSlots, _clientId);

            NumberOfSlots = null;

            UpdatePage();
        }

        private bool AddSlotsCanExecute(object parameter)
        {
            var result = int.TryParse(NumberOfSlots, out var numberOfSlots);

            return result && numberOfSlots > 0;
        }

        private void UpdatePage()
        {
            _clientDto = _managementClient.Get(_clientId);
            if (Items == null)
            {
                Items = new ObservableCollection<SlotDto>();
            }

            Items.Clear();

            foreach (var slot in _clientDto.Slots) 
            { 
                Items.Add(slot); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
