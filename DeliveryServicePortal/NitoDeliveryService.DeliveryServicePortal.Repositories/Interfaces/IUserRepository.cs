using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, int>
    {
        Task<User> ReadByLogin(string login);
    }
}
