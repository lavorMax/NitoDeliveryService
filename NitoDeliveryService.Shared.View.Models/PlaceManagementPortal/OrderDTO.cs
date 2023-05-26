using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.View.Models.DeliveryServicePortal;
using System.Collections.Generic;
using System.ComponentModel;

namespace NitoDeliveryService.Shared.View.Models.PlaceManagementPortal
{
    public class OrderDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public string Adress { get; set; }
        public int PlaceViewId { get; set; }
        public decimal DeliveryPrice { get; set; }

        public UserDto User { get; set; }
        public List<DishOrderDTO> DishOrders { get; set; }

        public override string ToString()
        {
            return $"Order #{Id}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
