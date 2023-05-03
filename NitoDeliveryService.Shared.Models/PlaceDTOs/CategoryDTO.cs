using System.Collections.Generic;

namespace NitoDeliveryService.Shared.Models.PlaceDTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<DishDTO> Dishes { get; set; }
    }
}
