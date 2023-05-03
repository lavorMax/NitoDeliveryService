using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class PlaceCategoryRepository : BaseRepository<PlaceCategory, int>, IPlaceCategoryRepository
    {
        public PlaceCategoryRepository(IDbContextFactory<PlaceManagementDbContext> contextfactory) : base(contextfactory.CreateDbContext())
        {
        }
    }
}
