using ManagementPortalWPF.HttpClients;
using ManagementPortalWPF.Services;
using ManagementPortalWPF.View;
using ManagementPortalWPF.ViewModels;
using Microsoft.Extensions.Configuration;
using NitoDeliveryService.Shared.HttpClients;
using System.IO;
using System.Windows;

namespace ManagementPortalWPF
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
            IManagementPortalClient client = new ManagementPortalClient(configuration);


            NavigationService navigation = new NavigationService();

            var eVM = new EnterViewModel(authClient, navigation, client);
            var mVM = new MainViewModel(navigation, client);

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
            navigation.ManagementClient = client;


            navig = navigation;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            navig.ShowEnter();
        }
    }
}
