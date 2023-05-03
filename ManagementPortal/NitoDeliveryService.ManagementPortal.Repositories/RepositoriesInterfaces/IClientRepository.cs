using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces
{
    public interface IClientRepository : IBaseRepository<Client, int>
    {
        Task<Client> ReadWithIncludes(int id);
    }
}
