using NitoDeliveryService.Shared.Models.DTOs;
using NitoDeliveryService.Shared.View.Models.ManagementPortal;
using System.Collections.Generic;

namespace ManagementPortalWPF.HttpClients
{
    public interface IManagementPortalClient
    {
        string Token { get; set; }

        void CreateCustomer(ClientDto client);
        void RemoveCustomer(int clientId);
        ClientDto Get(int clientId);
        IEnumerable<ClientDto> GetAll();
        void CreateSlot(int number, int clientId);
        void DeleteSlot(int slotId);
        void InitSlot(InitializeSlotRequest slotRequest);
        void DeinitSlot(int slotId);
        Auth0CredentialsResponse GetCreds(int slotId);
    }
}
