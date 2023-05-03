using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Interfaces;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlaceManagementDbContext _context;

        public UnitOfWork(IDbContextFactory<PlaceManagementDbContext> contextfactory)
        {
            _context = contextfactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
