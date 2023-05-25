using ManagementPortalWPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace ManagementPortalWPF.View
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private readonly INavigationService _navService;
        private readonly int _clientId;
        public ClientWindow(INavigationService navService, int clientId)
        {
            _navService = navService;
            _clientId = clientId;

            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _navService.CloseClient(_clientId, true);
        }
    }
}
