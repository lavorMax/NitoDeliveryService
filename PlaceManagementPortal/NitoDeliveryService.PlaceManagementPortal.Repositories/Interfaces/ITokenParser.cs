using NitoDeliveryService.PlaceManagementPortal.Entities;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories
{
    public interface ITokenParser
    {
        UserMetadata GetMetadata();
    }
}
