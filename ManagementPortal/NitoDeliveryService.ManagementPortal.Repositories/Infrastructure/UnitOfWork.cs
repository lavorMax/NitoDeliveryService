using NiteDeliveryService.Shared.DAL.Interfaces;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Repositories.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ManagementPortalDbContext _context;

        public UnitOfWork(ManagementPortalDbContext context)
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