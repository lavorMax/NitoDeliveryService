using NiteDeliveryService.Shared.DAL;
using NitoDeliveryService.Shared.Models.Models;
using System.Collections.Generic;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class Order : BaseEntity<int>
    {
        public int PlaceId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string Adress { get; set; }
        public int PlaceViewId{ get; set; }

        public PlaceView PlaceViews { get; set; }
        public User User { get; set; }


        public IEnumerable<DishOrder> DishOrders { get; set; }
    }
}
