namespace NitoDeliveryService.Shared.Models.PlaceDTOs
{
    public class PaymentConfigurationDTO
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public decimal Price { get; set; }
        public int MaxRange { get; set; }
    }
}
