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
        private const double EarthRadiusInMeters = 6371000;
        public PlaceViewRepository(DeliveryServiceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PlaceView>> GetPossibleToDeliverPlaces(double Latitude, double Longitude)
        {
            var result = await _context.Set<PlaceView>()
                .Where(p => Math.Acos(
                    Math.Sin(Latitude * Math.PI / 180) * Math.Sin(p.Latitude * Math.PI / 180) +
                    Math.Cos(Latitude * Math.PI / 180) * Math.Cos(p.Latitude * Math.PI / 180) *
                    Math.Cos(p.Longitude * Math.PI / 180 - Longitude * Math.PI / 180)
                ) * EarthRadiusInMeters <= p.DeliveryRange && !p.Deleted)
                .ToListAsync().ConfigureAwait(false);

            return result;
        }

        public async Task<PlaceView> ReadByPlaceAndClientId(int clientId, int placeId)
        {
            var result = await _context.Set<PlaceView>()
                .Where(p => p.ClientId == clientId && p.PlaceId == placeId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return result;
        }
    }
}
