using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Repositories.Repositories
{
    public class ClientRepository : BaseRepository<Client, int>, IClientRepository
    {
        public ClientRepository(ManagementPortalDbContext context) : base(context)
        {
        }

        public async Task<Client> ReadWithIncludes(int id)
        {
            try
            {
                return await _context.Set<Client>()
                    .Include(c => c.Slots)
                    .FirstAsync(c => c.Id == id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
