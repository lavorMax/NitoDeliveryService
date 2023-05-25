using Microsoft.EntityFrameworkCore;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces
{
    public interface IOverridingDbContextFactory<out T> : IDbContextFactory<T> where T:DbContext
    {
        void OverrideClientId(int clientId);
    }
}
