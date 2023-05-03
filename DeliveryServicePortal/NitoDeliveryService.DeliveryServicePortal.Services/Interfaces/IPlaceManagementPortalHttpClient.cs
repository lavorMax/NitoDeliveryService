using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IPlaceManagementPortalHttpClient
    {
        Task<PlaceDTO> Get(int placeId, int clientId);
    }
}
