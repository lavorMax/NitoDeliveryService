namespace NitoDeliveryService.PlaceManagementPortal.Services.Infrasctructure
{
    public class DeliveryServiceOptions
    {
        public string DeliveryServiceURL { get; set; }
        public string ChangeOrderStatusEndpoint { get; set; }
        public string GetOrdersEndpoint { get; set; }
        public string GetOrderEndpoint { get; set; }
        public string CreatePlaceEndpoint { get; set; }
        public string UpdatePlaceEndpoint { get; set; }
        public string DeletePlaceEndpoint { get; set; }
    }
}
