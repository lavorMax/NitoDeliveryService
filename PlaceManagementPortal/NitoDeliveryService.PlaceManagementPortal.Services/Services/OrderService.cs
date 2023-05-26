using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDeliveryServiceHttpClient _deliveryServiceHttpClient;
        private readonly IAuth0Client _auth0Client;

        public OrderService(IDeliveryServiceHttpClient deliveryServiceHttpClient, IAuth0Client auth0Client)
        {
            _deliveryServiceHttpClient = deliveryServiceHttpClient;
            _auth0Client = auth0Client;
        }

        public async Task ChangeStatusToClosed(int orderId)
        {
            await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Closed);
        }

        public async Task ChangeStatusToDelivering(int orderId)
        {
            await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Delivering);
        }

        public async Task ChangeStatusToPrepearing(int orderId)
        {
            await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Prepearing);
        }

        public async Task<IEnumerable<OrderDTO>> GetActiveOrders()
        {
            var userMetadata = await _auth0Client.GetMetadata();

            var activeOrders = await _deliveryServiceHttpClient.GetOrders(userMetadata.PlaceId, userMetadata.ClientId, true);

            if(activeOrders == null)
            {
                throw new Exception("Error getting active orders");
            }

            return activeOrders;
        }

        public async Task<OrderDTO> GetOrder(int orderId)
        {
            var activeOrder = await _deliveryServiceHttpClient.GetOrder(orderId);

            if (activeOrder == null)
            {
                throw new Exception("Error getting order");
            }

            return activeOrder;
        }
    }
}
