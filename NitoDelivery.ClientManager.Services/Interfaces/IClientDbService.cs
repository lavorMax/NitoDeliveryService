using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Interfaces
{
    public interface IClientDbService
    {
        Task CreateDb(int clientId);

        Task RemoveDb(int clientId);
    }
}
