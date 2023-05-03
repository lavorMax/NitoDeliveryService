using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.Shared.Models.DTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Interfaces
{
    public interface ISlotService
    {
        Task CreateSlots(int clientId, int number = 1);
        Task<Auth0CredentialsResponse> InitializeSlot(InitializeSlotRequest request);
        Task<Auth0CredentialsResponse> GetCredentials(int slotId);
        Task RemoveSlots(int id);
        Task DeinitializeSlot(int id);
    }
}
