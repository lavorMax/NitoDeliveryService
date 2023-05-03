using NiteDeliveryService.Shared.DAL;
using NitoDeliveryService.Shared.Models.Models;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class CategoryView : BaseEntity<int>
    {
        public int PlaceViewId { get; set; }
        public PlaceCategories Category { get; set; }

        public PlaceView PlaceView { get; set; }
    }
}
