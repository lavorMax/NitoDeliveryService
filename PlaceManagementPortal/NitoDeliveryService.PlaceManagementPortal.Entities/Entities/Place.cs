using NiteDeliveryService.Shared.DAL;
using System.Collections.Generic;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class Place : BaseEntity<int>
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SlotId { get; set; }
        public string Address { get; set; }

        public IEnumerable<Dish> Dishes { get; set; }
        public IEnumerable<PaymentConfiguration> PaymentConfigurations { get; set; }
    }
}
