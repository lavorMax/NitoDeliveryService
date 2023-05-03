using NiteDeliveryService.Shared.DAL;
using System.Collections.Generic;

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
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public IEnumerable<CategoryView> Categories { get; set; }
    }
}
