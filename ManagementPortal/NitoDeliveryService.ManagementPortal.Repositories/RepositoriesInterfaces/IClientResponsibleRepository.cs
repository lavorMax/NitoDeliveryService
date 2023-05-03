using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces
{
    public interface IClientResponsibleRepository : IBaseRepository<ClientResponsible, int>
    {
        Task<ClientResponsible> ReadWithIncludes(int id);
    }
}
