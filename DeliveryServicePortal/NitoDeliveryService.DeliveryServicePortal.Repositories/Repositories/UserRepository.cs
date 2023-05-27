using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository(DeliveryServiceDbContext context) : base(context)
        {
        }

        public async Task<User> ReadByLogin(string login)
        {
            var result = await _context.Set<User>()
                .FirstOrDefaultAsync(i => i.Email == login).ConfigureAwait(false);

            return result;
        }
    }
}
