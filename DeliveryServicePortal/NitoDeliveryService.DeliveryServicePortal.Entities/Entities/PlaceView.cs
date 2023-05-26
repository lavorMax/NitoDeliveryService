using NiteDeliveryService.Shared.DAL;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class PlaceView : BaseEntity<int>
    {
        public int PlaceId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public int DeliveryRange { get; set; }
        public bool Deleted { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
