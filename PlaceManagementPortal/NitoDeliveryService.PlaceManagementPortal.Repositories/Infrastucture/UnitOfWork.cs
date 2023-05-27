using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlaceManagementDbContext _context;

        public UnitOfWork(IOverridingDbContextFactory<PlaceManagementDbContext> contextfactory)
        {
            _context = contextfactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
