using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.Services;
using NitoDeliveryService.Shared.HttpClients;
using NitoDeliveryService.Shared.View;
using NitoDeliveryService.Shared.View.Models.DeliveryServicePortal;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DeliveryServiceWPF.ViewModel
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IDeliveryServiceHttpClient _client;
        private readonly IAuthClient _authClient;


        private string email;
        private string password;
        private string phone;
        private string surname;
        private string name;
        private Visibility incorrectCredentialsVisibility;

        public ICommand EnterCommand { get; }

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
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

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                if (surname != value)
                {
                    surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged(nameof(Phone));
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

        public RegistrationViewModel(IAuthClient authClient, INavigationService navigationService, IDeliveryServiceHttpClient managementClient)
        {
            EnterCommand = new Command(ExecuteEnterCommand, CanExecuteEnterCommand);
            IncorrectCredentialsVisibility = Visibility.Hidden;

            _authClient = authClient;
            _navigationService = navigationService;
            _client = managementClient;
        }

        private async void ExecuteEnterCommand(object parameter)
        {
            var user = new UserDto()
            {
                Name = Name,
                Surname = Surname,
                Phone = Phone,
                Email = email,
                Password = password
            };

            var userId = _client.CreateUser(user);

            if(userId != -1)
            {
                var token = await _authClient.Authenticate(Email, Password);

                if (token != null)
                {
                    _client.SetupToken(token);
                    _navigationService.ShowMain(userId);
                }
                else
                {
                    IncorrectCredentialsVisibility = Visibility.Visible;
                }
            }
        }

        private bool CanExecuteEnterCommand(object parameter)
        {
            return !string.IsNullOrEmpty(Password) 
                && !string.IsNullOrEmpty(Email)
                && !string.IsNullOrEmpty(Name)
                && !string.IsNullOrEmpty(Surname)
                && !string.IsNullOrEmpty(Phone);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
