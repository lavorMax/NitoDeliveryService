using NitoDeliveryService.PlaceManagementPortal.Entities;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces
{
    public interface IAuth0Client
    {
        Task<UserMetadata> GetMetadata();
    }
}
