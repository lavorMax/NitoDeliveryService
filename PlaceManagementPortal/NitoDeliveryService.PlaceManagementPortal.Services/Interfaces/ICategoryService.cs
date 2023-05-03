using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface ICategoryService
    {
        Task CreateNewCoategory(CategoryDTO category);
        Task RemoveCategory(int categoryId);
    }
}
