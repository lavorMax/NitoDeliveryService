using NitoDeliveryService.Shared.Models.Models;

namespace NitoDeliveryService.Shared.Models.PlaceDTOs
{
    public class PlaceCategoryDTO
    {
        public int Id { get; set; }
        public PlaceCategories Category { get; set; }
        public int PlaceId { get; set; }
    }
}
