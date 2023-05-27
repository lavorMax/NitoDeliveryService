using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NitoDeliveryService.DeliveryServicePortal.Services.Infrastructure
{
    public static class CoordinateGetter
    {
        private const double EarthRadiusInMeters = 6371000;
        public static async Task<(double, double)> GetCoordinates(string address)
        {
            var geocoder = new ForwardGeocoder();
            var addressResponse = await geocoder.Geocode(new ForwardGeocodeRequest { queryString = address, BreakdownAddressElements = true }).ConfigureAwait(false);

            var addresDecoded = addressResponse.FirstOrDefault();
            if (addresDecoded == null)
            {
                throw new Exception("Error getting address");
            }

            return (addresDecoded.Latitude, addresDecoded.Longitude);
        }

        public static double GetDistance(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            return Math.Acos(
                    Math.Sin(Latitude1 * Math.PI / 180) * Math.Sin(Latitude2 * Math.PI / 180) +
                    Math.Cos(Latitude1 * Math.PI / 180) * Math.Cos(Latitude2 * Math.PI / 180) *
                    Math.Cos(Longitude2 * Math.PI / 180 - Longitude1 * Math.PI / 180)
                ) * EarthRadiusInMeters;
        }
    }
}
