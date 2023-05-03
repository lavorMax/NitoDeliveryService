using System.Collections.Generic;

namespace NitoDeliveryService.ManagementPortal.Models.DTOs
{
    public class ClientResponsibleDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string SurName { get; set; }
        public int ClientId { get; set; }

        public IEnumerable<ClientPhoneDto> ClientPhones { get; set; }
    }
}
