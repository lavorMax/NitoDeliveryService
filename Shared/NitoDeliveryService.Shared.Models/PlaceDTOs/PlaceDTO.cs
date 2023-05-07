using System.Collections.Generic;

namespace NitoDeliveryService.Shared.Models.PlaceDTOs
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public IEnumerable<DishDTO> Dishes { get; set; }
        public IEnumerable<PaymentConfigurationDTO> PaymentConfigurations { get; set; }
    }
}
