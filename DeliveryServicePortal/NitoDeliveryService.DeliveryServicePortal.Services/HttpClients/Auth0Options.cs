namespace NitoDeliveryService.DeliveryServicePortal.API.Infrastructure
{
    public class Auth0Options
    {
        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SigingSecret { get; set; }
    }
}
