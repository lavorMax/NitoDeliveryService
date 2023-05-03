using NitoDeliveryService.Shared.Models.DTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Interfaces
{
    public interface IPlaceManagementPortalHttpClient
    {
        Task InitializeSlot(int clientId, InitializeSlotRequest request);
        Task DeinitializeSlot(int clientId, int slotId);
    }
}
