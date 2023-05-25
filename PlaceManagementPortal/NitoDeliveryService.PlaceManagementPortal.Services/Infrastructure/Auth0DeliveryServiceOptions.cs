namespace NitoDeliveryService.PlaceManagementPortal.Services.Infrastructure
{
    public class Auth0DeliveryServiceOptions
    {
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }

        public string Domain { get; set; }
        public string Audience { get; set; }
    }
}
