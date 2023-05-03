using NiteDeliveryService.Shared.DAL;
using System.Collections.Generic;

namespace NitoDeliveryService.ManagementPortal.Entities.Entities
{
    public class Client : BaseEntity<int>
    {
        public string Title { get; set; }

        public IEnumerable<ClientResponsible> Responsibles { get; set; }
        public IEnumerable<Slot> Slots { get; set; }
    }
}
