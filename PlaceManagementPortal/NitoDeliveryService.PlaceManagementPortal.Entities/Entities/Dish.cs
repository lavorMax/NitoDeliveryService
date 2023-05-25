using NiteDeliveryService.Shared.DAL;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class Dish : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PlaceId { get; set; }
        public decimal Price { get; set; }

        public Place Place { get; set; }
    }
}
