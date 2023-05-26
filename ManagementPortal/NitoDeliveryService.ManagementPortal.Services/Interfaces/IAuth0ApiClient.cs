using NitoDeliveryService.ManagementPortal.Models.DTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Interfaces
{
    public interface IAuth0ApiClient
    {
        Task DeleteUser(string userId);
        Task<Auth0CredentialsResponse> CreateUser(string username, int clientId, int slotId);
    }
}
