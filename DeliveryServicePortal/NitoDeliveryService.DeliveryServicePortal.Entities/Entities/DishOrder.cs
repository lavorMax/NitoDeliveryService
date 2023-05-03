using NiteDeliveryService.Shared.DAL;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class DishOrder : BaseEntity<int>
    {
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public int Number { get; set; }
        public decimal DishPrice { get; set; }
        public string DishName { get; set; }

        public Order Order { get; set; }
    }
}
