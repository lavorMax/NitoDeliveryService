using NitoDeliveryService.Shared.HttpClients;
using System.Threading.Tasks;

namespace DeliveryServiceWPF.HttpClients
{
    public class Auth0RegisterClient : Auth0Client, IAuth0RegisterClient
    {
        public Auth0RegisterClient(Auth0Options options) : base(options)
        {
        }
        public Task<string> CreateUser(string email, string password, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
