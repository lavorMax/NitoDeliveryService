using NiteDeliveryService.Shared.DAL;
using System.Collections.Generic;

namespace NitoDeliveryService.PlaceManagementPortal.Entities.Entities
{
    public class User : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
