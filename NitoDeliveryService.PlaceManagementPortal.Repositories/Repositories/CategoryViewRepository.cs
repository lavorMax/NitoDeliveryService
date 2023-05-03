using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class CategoryViewRepository : BaseRepository<CategoryView, int>, ICategoryViewRepository
    {
        public CategoryViewRepository(DeliveryServiceDbContext context) : base(context)
        {
        }
    }
}
