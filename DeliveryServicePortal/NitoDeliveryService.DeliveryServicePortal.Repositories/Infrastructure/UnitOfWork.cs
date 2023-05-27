using NiteDeliveryService.Shared.DAL.Interfaces;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeliveryServiceDbContext _context;

        public UnitOfWork(DeliveryServiceDbContext context)
        {
            _context = context;
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
