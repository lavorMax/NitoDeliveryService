using NitoDeliveryService.Shared.HttpClients;
using System.Threading.Tasks;

namespace DeliveryServiceWPF.HttpClients
{
    public interface IAuth0RegisterClient : IAuthClient
    {
        Task<string> CreateUser(string email, string password, int userId);
    }
}
