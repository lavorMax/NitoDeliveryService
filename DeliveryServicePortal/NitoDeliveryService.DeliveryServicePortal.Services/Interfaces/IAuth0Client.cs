using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.DeliveryServicePortal.Services.Interfaces
{
    public interface IAuth0Client
    {
        Task CreateUser(UserDTO user);
    }
}
