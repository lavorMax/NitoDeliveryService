using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;

namespace NitoDeliveryService.ManagementPortal.Repositories.Repositories
{
    public class ClientPhoneRepository : BaseRepository<ClientPhone, int>, IClientPhoneRepository
    {
        public ClientPhoneRepository(ManagementPortalDbContext context) : base(context)
        {
        }
    }
}
