using NitoDeliveryService.Shared.Models.DTOs;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IPlaceService
    {
        Task CreateNewPlace(InitializeSlotRequest place);
        Task RemovePlace(int placeId);
        Task UpdatePlace(PlaceDTO place);
        Task<PlaceDTO> GetPlace(int placeId);
        Task<PlaceDTO> GetPlace();
    }
}
