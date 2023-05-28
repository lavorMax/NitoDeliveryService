using Microsoft.Extensions.Configuration;
using NitoDeliveryService.Shared.HttpClients;
using PlaceManagementPortalWPF.HttpClients;
using PlaceManagementPortalWPF.Services;
using PlaceManagementPortalWPF.View;
using PlaceManagementPortalWPF.ViewModel;
using System.IO;
using System.Windows;

namespace PlaceManagementPortalWPF
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
            IPlaceManagementPortal client = new PlaceManagementPortal(configuration);


            NavigationService navigation = new NavigationService();

            var eVM = new EnterViewModel(authClient, navigation, client);
            var mVM = new MainViewModel(client, navigation);
            var pcVM = new PlaceConfigurationViewModel(client, navigation);

            var eWindow = new EnterWindow()
            {
                DataContext = eVM
            };

            var mWindow = new MainWindow()
            {
                DataContext = mVM
            };


            navigation.EnterWindow = eWindow;
            navigation.MainWindow = mWindow;
            navigation.MainVM = mVM;
            navigation.PlaceConfigurationVM = pcVM; 
            navigation.ManagementClient = client;


            navig = navigation;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            navig.ShowEnter();
        }
    }
}
