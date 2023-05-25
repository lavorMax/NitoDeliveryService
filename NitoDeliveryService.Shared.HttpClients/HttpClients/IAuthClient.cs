using System.Threading.Tasks;

namespace NitoDeliveryService.Shared.HttpClients
{
    public interface IAuthClient
    {
        Task<string> Authenticate(string login, string password);
    }
}
