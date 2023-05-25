using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IDishService
    {
        Task CreateNewDish(DishDTO dish);
        Task RemoveDish(int dishId);
    }
}
