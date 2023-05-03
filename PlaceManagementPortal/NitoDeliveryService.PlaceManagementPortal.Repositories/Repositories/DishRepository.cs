using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class DishRepository : BaseRepository<Dish, int>, IDishRepository
    {
        public DishRepository(IDbContextFactory<PlaceManagementDbContext> contextfactory) : base(contextfactory.CreateDbContext())
        {
        }
    }
}
