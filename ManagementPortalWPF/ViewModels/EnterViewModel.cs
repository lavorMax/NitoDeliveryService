using ManagementPortalWPF.HttpClients;
using ManagementPortalWPF.Services;
using NitoDeliveryService.Shared.HttpClients;
using NitoDeliveryService.Shared.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ManagementPortalWPF.ViewModels
{
    public class EnterViewModel : INotifyPropertyChanged
    {
        private string login = "lavor.maxim@gmail.com";
        private string password = "maksym32";
        private Visibility incorrectCredentialsVisibility;

        private readonly IAuthClient _authClient;
        private readonly INavigationService _navigationService;
        private readonly IManagementPortalClient _managementClient;

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

        public ICommand EnterCommand { get; }

        public EnterViewModel(IAuthClient authClient, INavigationService navService, IManagementPortalClient managementClient)
        {
            EnterCommand = new Command(ExecuteEnterCommand, CanExecuteEnterCommand);
            IncorrectCredentialsVisibility = Visibility.Hidden;

            _authClient = authClient;
            _navigationService = navService;
            _managementClient = managementClient;
        }

        private async void ExecuteEnterCommand(object parameter)
        {
            var token = await _authClient.Authenticate(Login, Password);

            if (token != null)
            {
                _managementClient.Token = token;
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
