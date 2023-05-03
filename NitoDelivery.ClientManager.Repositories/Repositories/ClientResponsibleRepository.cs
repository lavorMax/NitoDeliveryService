using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Repositories.Repositories
{
    public class ClientResponsibleRepository : BaseRepository<ClientResponsible, int>, IClientResponsibleRepository
    {
        public ClientResponsibleRepository(ManagementPortalDbContext context) : base(context){}

        public async Task<ClientResponsible> ReadWithIncludes(int id)
        {
            try
            {
                return await _context.Set<ClientResponsible>()
                    .Include(c => c.ClientPhones)
                    .FirstAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
