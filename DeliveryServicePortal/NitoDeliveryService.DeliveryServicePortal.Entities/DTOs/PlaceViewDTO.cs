using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Collections.Generic;

namespace NitoDeliveryService.PlaceManagementPortal.Models.DTOs
{
    public class PlaceViewDTO
    {
        public int PlaceId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
