using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
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
        private readonly ITokenParser _tokenParser;

        public OrderService(IDeliveryServiceHttpClient deliveryServiceHttpClient, ITokenParser tokenParser)
        {
            _deliveryServiceHttpClient = deliveryServiceHttpClient;
            _tokenParser = tokenParser;
        }

        public async Task ChangeStatusToClosed(int orderId)
        {
            var result = await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Closed);

            if (!result)
            {
                throw new Exception("Error updating order status");
            }
        }

        public async Task ChangeStatusToDelivering(int orderId)
        {
            var result = await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Delivering);

            if (!result)
            {
                throw new Exception("Error updating order status");
            }
        }

        public async Task ChangeStatusToPrepearing(int orderId)
        {
            var result = await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Prepearing);

            if (!result)
            {
                throw new Exception("Error updating order status");
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetActiveOrders()
        {
            var userMetadata = _tokenParser.GetMetadata();

            var activeOrders = await _deliveryServiceHttpClient.GetOrders(userMetadata.PlaceId, userMetadata.ClientId, true);

            if(activeOrders == null)
            {
                throw new Exception("Error getting active orders");
            }

            return activeOrders;
        }

        public async Task<IEnumerable<OrderDTO>> GetArchievedOrders()
        {
            var userMetadata = _tokenParser.GetMetadata();

            var archievedOrders = await _deliveryServiceHttpClient.GetOrders(userMetadata.PlaceId, userMetadata.ClientId, false);

            if (archievedOrders == null)
            {
                throw new Exception("Error getting archieved orders");
            }

            return archievedOrders;
        }
    }
}
