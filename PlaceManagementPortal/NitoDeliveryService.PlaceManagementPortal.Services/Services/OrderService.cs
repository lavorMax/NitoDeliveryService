using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDeliveryServiceHttpClient _deliveryServiceHttpClient;
        private readonly IAuth0Client _auth0Client;
        private readonly IPlaceRepository _placeRepository;

        public OrderService(IDeliveryServiceHttpClient deliveryServiceHttpClient, IAuth0Client auth0Client, IPlaceRepository placeRepository)
        {
            _deliveryServiceHttpClient = deliveryServiceHttpClient;
            _auth0Client = auth0Client;
            _placeRepository = placeRepository;
        }

        public async Task ChangeStatusToClosed(int orderId)
        {
            await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Closed).ConfigureAwait(false);
        }

        public async Task ChangeStatusToDelivering(int orderId)
        {
            await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Delivering).ConfigureAwait(false);
        }

        public async Task ChangeStatusToPrepearing(int orderId)
        {
            await _deliveryServiceHttpClient.ChangeStatus(orderId, OrderStatuses.Prepearing).ConfigureAwait(false);
        }

        public async Task<IEnumerable<OrderDTO>> GetActiveOrders()
        {
            var userMetadata = await _auth0Client.GetMetadata().ConfigureAwait(false);

            var place = await _placeRepository.ReadWithIncludesBySlotId(userMetadata.PlaceId).ConfigureAwait(false);

            var activeOrders = await _deliveryServiceHttpClient.GetOrders(place.Id, userMetadata.ClientId, true).ConfigureAwait(false);

            if(activeOrders == null)
            {
                throw new ExternalException("Error getting active orders");
            }

            return activeOrders;
        }

        public async Task<OrderDTO> GetOrder(int orderId)
        {
            var activeOrder = await _deliveryServiceHttpClient.GetOrder(orderId).ConfigureAwait(false);

            if (activeOrder == null)
            {
                throw new ExternalException("Error getting order");
            }

            return activeOrder;
        }
    }
}
