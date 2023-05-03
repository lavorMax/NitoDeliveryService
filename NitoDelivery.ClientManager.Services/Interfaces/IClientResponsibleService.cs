using NitoDeliveryService.ManagementPortal.Models.DTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Interfaces
{
    public interface IClientResponsibleService
    {
        Task AddClientResponsible(ClientResponsibleDto responsible);
        Task RemoveClientResponsible(int id);
        Task AddClientResponsiblePhone(ClientPhoneDto clientPhone);
        Task RemoveClientPhone(int id);
    }
}
