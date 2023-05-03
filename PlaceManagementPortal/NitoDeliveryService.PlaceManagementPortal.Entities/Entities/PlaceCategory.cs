using NiteDeliveryService.Shared.DAL;
using NitoDeliveryService.Shared.Models.Models;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class PlaceCategory : BaseEntity<int>
    {
        public PlaceCategories Category { get; set; }
        public int PlaceId { get; set; }

        public Place Place { get; set; }
    }
}
