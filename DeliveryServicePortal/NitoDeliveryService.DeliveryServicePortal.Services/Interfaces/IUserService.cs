using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUser(UserDTO userDto);
        Task UpdateUser(UserDTO userDto);
        Task<UserDTO> GetUser(string userLogin);
    }
}
