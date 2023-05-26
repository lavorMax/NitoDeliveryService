using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces
{
    public interface IPlaceRepository : IBaseRepository<Place, int>
    {
        Task<Place> ReadWithIncludes(int id);

        Task<Place> ReadWithIncludesBySlotId(int id);
        Task<bool> DeleteBySlotId(int slotId);
    }
}
