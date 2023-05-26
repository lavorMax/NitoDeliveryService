using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.Shared.Models.Models;
using System.Collections.Generic;

namespace NitoDeliveryService.Shared.Models.PlaceDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public string Adress { get; set; }
        public int PlaceViewId { get; set; }
        public decimal DeliveryPrice { get; set; }


        public UserDTO User { get; set; }
        public IEnumerable<DishOrderDTO> DishOrders { get; set; }
    }
}
