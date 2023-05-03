namespace NitoDeliveryService.ManagementPortal.Models.DTOs
{
    public class ClientPhoneDto
    {
        public int Id { get; set; }
        public string Phone { get; set; }

        public int ClientResponsibleId { get; set; }
    }
}
