using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class PlaceRepository : BaseRepository<Place, int>, IPlaceRepository
    {
        public PlaceRepository(IDbContextFactory<PlaceManagementDbContext> contextfactory) : base(contextfactory.CreateDbContext())
        {
        }

        public async Task<bool> DeleteBySlotId(int slotId)
        {
            try
            {
                var result = await _context.Set<Place>().FirstAsync(p => p.SlotId == slotId);
                if (result != null)
                {
                    _context.Set<Place>().Remove(result);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Place> ReadWithIncludes(int id)
        {
            try
            {
                return await _context.Set<Place>()
                    .Include(p => p.PlaceCategories)
                    .Include(p => p.PaymentConfigurations)
                    .Include(p => p.Categories)
                    .ThenInclude(c => c.Dishes)
                    .FirstAsync(p => p.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async new Task<bool> Update(Place entity)
        {
            try
            {
                var result = await Read(entity.Id);
                if (result != null)
                {
                    PropertyInfo[] properties = typeof(Place).GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.Name != "SlotId")
                        {
                            property.SetValue(result, property.GetValue(entity));
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
