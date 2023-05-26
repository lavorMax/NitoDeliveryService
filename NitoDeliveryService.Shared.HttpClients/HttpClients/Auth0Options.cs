namespace NitoDeliveryService.Shared.HttpClients
{
    public class Auth0Options
    {
        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string Realm { get; set; }
    }
}
