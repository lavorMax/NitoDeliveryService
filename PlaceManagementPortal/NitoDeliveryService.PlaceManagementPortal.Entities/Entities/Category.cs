using NiteDeliveryService.Shared.DAL;
using System.Collections.Generic;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class Category : BaseEntity<int>
    {
        public int PlaceId { get; set; }
        public string Name { get; set; }

        public Place Place { get; set; }
        public IEnumerable<Dish> Dishes { get; set; }
    }
}
