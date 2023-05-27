using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.DeliveryServicePortal.Services.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class OrderService : IOrderService
    {

        private readonly IPlaceManagementPortalHttpClient _placeManagerHttpClient;
        private readonly IOrderRepository _orderRepository;
        private readonly IPlaceViewRepository _placeViewRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IPlaceManagementPortalHttpClient placeManagerHttpClient, IPlaceViewRepository placeViewRepository, IOrderRepository orderRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _placeManagerHttpClient = placeManagerHttpClient;
            _placeViewRepository = placeViewRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateOrder(OrderDTO order)
        {
            var orderEntity = _mapper.Map<OrderDTO, Order>(order);

            var place = await _placeManagerHttpClient.Get(order.PlaceId, order.ClientId).ConfigureAwait(false);
            var placeView = await _placeViewRepository.Read(order.PlaceViewId).ConfigureAwait(false);

            var (addressLatitude, addressLongitude) = await CoordinateGetter.GetCoordinates(order.Adress).ConfigureAwait(false);

            var distance = CoordinateGetter.GetDistance(addressLatitude, addressLongitude, placeView.Latitude, placeView.Longitude);

            var paymentConfig = place.PaymentConfigurations.OrderBy(i => i.MaxRange).FirstOrDefault(i => distance < i.MaxRange);

            orderEntity.DeliveryPrice = paymentConfig.Price;


            var result = await _orderRepository.Create(orderEntity).ConfigureAwait(false);
            if (result == null)
            {
                throw new Exception("Error creating order");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUser(int userId, bool onlyActiveOrders = true)
        {
            var allOrdersEntities = await _orderRepository.GetOrdersByUser(userId, onlyActiveOrders).ConfigureAwait(false);

            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(allOrdersEntities);

            return result;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByPlace(int clientId, int placeId, bool onlyActiveOrders = true)
        {
            var allOrdersEntities = await _orderRepository.GetOrdersByPlace(clientId, placeId, onlyActiveOrders).ConfigureAwait(false);

            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(allOrdersEntities);

            return result;
        }

        public async Task UpdateOrderStatus(int orderId, OrderStatuses status)
        {
            var orderEntity = await _orderRepository.Read(orderId).ConfigureAwait(false);

            orderEntity.OrderStatus = status;

            var result = await _orderRepository.Update(orderEntity).ConfigureAwait(false);
            if (!result)
            {
                throw new Exception("Error updating order");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task<OrderDTO> GetOrder(int orderId)
        {
            var order = await _orderRepository.ReadWithIncludes(orderId).ConfigureAwait(false);

            var result = _mapper.Map<Order, OrderDTO>(order);

            return result;
        }
    }
}
