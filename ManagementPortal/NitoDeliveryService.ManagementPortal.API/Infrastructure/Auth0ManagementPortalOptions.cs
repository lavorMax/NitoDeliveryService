namespace NitoDeliveryService.ManagementPortal.Services.Infrastructure
{
    public class Auth0ManagementPortalOptions
    {
        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ClientSecret { get; set; }
    }
}
