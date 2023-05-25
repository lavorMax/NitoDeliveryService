using Microsoft.EntityFrameworkCore;
using NiteDeliveryService.Shared.DAL.Implemetations;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Repositories
{
    public class OrderRepository : BaseRepository<Order, int>, IOrderRepository
    {
        public OrderRepository(DeliveryServiceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByPlace(int clientId, int placeId, bool onlyActiveOrders = true)
        {
            var result = await _context.Set<Order>()
                .Where(o => o.ClientId == clientId && o.PlaceId == placeId && onlyActiveOrders ? (o.OrderStatus != OrderStatuses.Finished || o.OrderStatus != OrderStatuses.Closed) : true)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUser(int userId, bool onlyActiveOrders = true)
        {
            var result = await _context.Set<Order>()
                .Where(o => o.UserId == userId && onlyActiveOrders ? (o.OrderStatus != OrderStatuses.Finished || o.OrderStatus != OrderStatuses.Closed) : true)
                .ToListAsync();

            return result;
        }

        public async Task<Order> ReadWithIncludes(int orderId)
        {
            var result = await _context.Set<Order>()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return result;
        }
    }
}
