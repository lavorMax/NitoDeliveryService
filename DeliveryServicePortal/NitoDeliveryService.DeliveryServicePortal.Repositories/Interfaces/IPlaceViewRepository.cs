using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces
{
    public interface IPlaceViewRepository : IBaseRepository<PlaceView, int>
    {
        Task<IEnumerable<PlaceView>> GetPossibleToDeliverPlaces(double Latitude, double Longitude);
        Task<PlaceView> ReadByPlaceAndClientId(int clientId, int placeId);
    }
}
