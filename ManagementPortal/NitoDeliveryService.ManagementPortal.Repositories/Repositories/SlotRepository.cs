using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;

namespace NitoDeliveryService.ManagementPortal.Repositories.Repositories
{
    public class SlotRepository : BaseRepository<Slot, int>, ISlotRepository
    {
        public SlotRepository(ManagementPortalDbContext context) : base(context) {}
    }
}
