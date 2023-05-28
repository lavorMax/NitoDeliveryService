using NitoDeliveryService.Shared.HttpClients;
using NitoDeliveryService.Shared.View;
using PlaceManagementPortalWPF.HttpClients;
using PlaceManagementPortalWPF.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PlaceManagementPortalWPF.ViewModel
{
    public class EnterViewModel : INotifyPropertyChanged
    {
        private readonly IAuthClient _authClient;
        private readonly INavigationService _navigationService;
        private readonly IPlaceManagementPortal _managementClient;


        private string login;
        private string password;
        private Visibility incorrectCredentialsVisibility;

        public ICommand EnterCommand { get; }

        public string Login
        {
            get { return login; }
            set
            {
                if (login != value)
                {
                    login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public Visibility IncorrectCredentialsVisibility
        {
            get { return incorrectCredentialsVisibility; }
            set
            {
                if (incorrectCredentialsVisibility != value)
                {
                    incorrectCredentialsVisibility = value;
                    OnPropertyChanged(nameof(IncorrectCredentialsVisibility));
                }
            }
        }

        public EnterViewModel(IAuthClient authClient, INavigationService navigationService, IPlaceManagementPortal managementClient)
        {
            EnterCommand = new Command(ExecuteEnterCommand, CanExecuteEnterCommand);
            IncorrectCredentialsVisibility = Visibility.Hidden;

            _authClient = authClient;
            _navigationService = navigationService;
            _managementClient = managementClient;
        }

        private async void ExecuteEnterCommand(object parameter)
        {
            var token = await _authClient.Authenticate(Login, Password);

            if (token != null)
            {
                _managementClient.SetupToken(token);
                _navigationService.ShowMain();
            }
            else
            {
                IncorrectCredentialsVisibility = Visibility.Visible;
            }
        }

        private bool CanExecuteEnterCommand(object parameter)
        {
            return !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Login);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
