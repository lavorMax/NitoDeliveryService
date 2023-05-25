namespace NitoDeliveryService.ManagementPortal.Models.DTOs
{
    public class SlotDto
    {
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public string ClientId { get; set; }
        public string ManagerLogin { get; set; }
        public string Name { get; set; }
    }
}
