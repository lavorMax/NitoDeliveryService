using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public interface ITokenParser
    {
        UserMetadata GetMetadata();
    }
}
