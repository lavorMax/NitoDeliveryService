using NiteDeliveryService.Shared.DAL;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class PaymentConfiguration : BaseEntity<int>
    {
        public int PlaceId { get; set; }
        public decimal Price { get; set; }
        public int MaxRange { get; set; }

        public Place Place { get; set; }
    }
}
