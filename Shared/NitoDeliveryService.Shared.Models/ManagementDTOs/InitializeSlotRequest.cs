namespace NitoDeliveryService.Shared.Models.DTOs
{
    public class InitializeSlotRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int SlotId { get; set; }
        public int ClientId { get; set; }
    }
}
