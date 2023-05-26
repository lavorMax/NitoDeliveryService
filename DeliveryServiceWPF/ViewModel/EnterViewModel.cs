using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.Services;
using NitoDeliveryService.Shared.HttpClients;
using NitoDeliveryService.Shared.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DeliveryServiceWPF.ViewModel
{
    public class EnterViewModel : INotifyPropertyChanged
    {
        private readonly IAuthClient _authClient;
        private readonly INavigationService _navigationService;
        private readonly IDeliveryServiceHttpClient _client;


        private string login = "maksym32@gmail.com";
        private string password = "maksym32";
        private Visibility incorrectCredentialsVisibility;

        public ICommand EnterCommand { get; }
        public ICommand RegisterCommand { get; }

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

        public EnterViewModel(IAuthClient authClient, INavigationService navigationService, IDeliveryServiceHttpClient managementClient)
        {
            EnterCommand = new Command(ExecuteEnterCommand, CanExecuteEnterCommand);
            RegisterCommand = new Command(Register);
            IncorrectCredentialsVisibility = Visibility.Hidden;

            _authClient = authClient;
            _navigationService = navigationService;
            _client = managementClient;
        }

        private void Register(object parameter)
        {
            _navigationService.ShowRegister();
        }

        private async void ExecuteEnterCommand(object parameter)
        {
            var token = await _authClient.Authenticate(Login, Password);

            if (token != null)
            {
                _client.SetupToken(token);

                var userId = _client.GetUser(Login).Id;

                _navigationService.ShowMain(userId);
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
