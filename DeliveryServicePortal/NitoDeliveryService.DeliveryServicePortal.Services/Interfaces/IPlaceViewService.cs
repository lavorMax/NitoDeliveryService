using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IPlaceViewService
    {
        Task CreatePlaceView(PlaceDTO placeDto);
        Task UpdatePlaceView(PlaceDTO placeDto);
        Task DeletePlaceView(int clientId, int placeId);
        Task<IEnumerable<PlaceViewDTO>> GetAllPossibleToDeliver(string adress);
        Task<PlaceDTO> Get(int placeId, int clientId);
    }
}
