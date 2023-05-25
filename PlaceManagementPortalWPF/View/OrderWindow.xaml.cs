using PlaceManagementPortalWPF.Services;
using System.Windows;

namespace PlaceManagementPortalWPF.View
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly INavigationService _navService;
        private readonly int _orderId;

        public OrderWindow(INavigationService navService, int orderId)
        {
            _navService = navService;
            _orderId = orderId;

            InitializeComponent();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _navService.CloseOrder(_orderId, true);
        }
    }
}
