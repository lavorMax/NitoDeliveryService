using NitoDeliveryService.ManagementPortal.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientDto> ReadFullClientById(int id);
        Task<IEnumerable<ClientDto>> ReadAllClients();
        Task CreateNewClient(ClientDto client);
        Task RemoveClient(int id);
    }
}
