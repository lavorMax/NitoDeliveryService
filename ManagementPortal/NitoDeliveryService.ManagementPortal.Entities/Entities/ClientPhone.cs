using NiteDeliveryService.Shared.DAL;

namespace NitoDeliveryService.ManagementPortal.Entities.Entities
{
    public class ClientPhone : BaseEntity<int>
    {
        public string Phone { get; set; }
        public int ClientResponsibleId { get; set; }
        public ClientResponsible ClientResponsible { get; set; }
    }
}
