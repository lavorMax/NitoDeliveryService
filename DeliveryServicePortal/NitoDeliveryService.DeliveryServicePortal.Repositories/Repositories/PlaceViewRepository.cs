using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class PlaceViewRepository : BaseRepository<PlaceView, int>, IPlaceViewRepository
    {
        public PlaceViewRepository(DeliveryServiceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PlaceView>> GetPossibleToDeliverPlaces(double Latitude, double Longitude)
        {
            var result = await _context.Set<PlaceView>()
                .Where(p => GetDistance(Latitude, Longitude, p.Latitude, p.Longitude) <= p.DeliveryRange 
                && p.Deleted)
                .ToListAsync();

            return result;
        }

        public async Task<PlaceView> ReadByPlaceAndClientId(int clientId, int placeId)
        {
            var result = await _context.Set<PlaceView>().Where(p => p.ClientId == clientId && p.PlaceId == placeId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<PlaceView>> SearchByName(double Latitude, double Longitude, string[] keys)
        {
            var result = await _context.Set<PlaceView>()
                .Where(p => GetDistance(Latitude, Longitude, p.Latitude, p.Longitude) <= p.DeliveryRange && keys.Any(k => p.Name.Contains(k)))
                .ToListAsync();

            return result;
        }

        private double GetDistance(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            return Math.Sqrt(Math.Pow(Latitude1 - Latitude2, 2) + Math.Pow(Longitude1 - Longitude2, 2));
        }
    }
}
