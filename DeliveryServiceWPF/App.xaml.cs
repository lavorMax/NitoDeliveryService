using DeliveryServiceWPF.HttpClients;
using DeliveryServiceWPF.Services;
using DeliveryServiceWPF.View;
using DeliveryServiceWPF.ViewModel;
using Microsoft.Extensions.Configuration;
using NitoDeliveryService.Shared.HttpClients;
using System.IO;
using System.Windows;

namespace DeliveryServiceWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly INavigationService navig;

        public App()
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var auth0Options = configuration.GetSection("Auth0Options").Get<Auth0Options>();

            IAuthClient authClient = new Auth0Client(auth0Options);
            IDeliveryServiceHttpClient client = new DeliveryServiceHttpClient(configuration);


            NavigationService navigation = new NavigationService(client);

            var eVM = new EnterViewModel(authClient, navigation, client);
            var mVM = new MainViewModel(navigation, client);
            var pcVM = new RegistrationViewModel(authClient, navigation, client);

            var eWindow = new EnterWindow()
            {
                DataContext = eVM
            };

            var mWindow = new MainWindow()
            {
                DataContext = mVM
            };

            var pcWindow = new RegistrationWindow()
            {
                DataContext = pcVM
            };


            navigation.EnterWindow = eWindow;
            navigation.MainWindow = mWindow;
            navigation.RegistrationWindow = pcWindow;
            navigation.EnterViewModel = eVM;
            navigation.MainViewModel = mVM;
            navigation.RegistrationViewModel = pcVM;


            navig = navigation;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            navig.ShowEnter();
        }
    }
}
