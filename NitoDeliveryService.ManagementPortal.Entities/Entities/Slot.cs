using NiteDeliveryService.Shared.DAL;

namespace NitoDeliveryService.ManagementPortal.Entities.Entities
{
    public class Slot : BaseEntity<int>
    {
        public bool IsUsed { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
