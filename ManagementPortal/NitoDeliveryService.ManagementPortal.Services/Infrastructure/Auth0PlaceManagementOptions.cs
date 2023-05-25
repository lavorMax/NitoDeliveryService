namespace NitoDeliveryService.ManagementPortal.Services.Infrastructure
{
    public class Auth0PlaceManagementOptions
    {
        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }

    }
}
