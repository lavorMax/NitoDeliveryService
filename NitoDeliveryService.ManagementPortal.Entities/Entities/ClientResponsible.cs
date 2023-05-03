using NiteDeliveryService.Shared.DAL;
using System.Collections.Generic;

namespace NitoDeliveryService.ManagementPortal.Entities.Entities
{
    public class ClientResponsible : BaseEntity<int>
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public IEnumerable<ClientPhone> ClientPhones { get; set; }
    }
}
