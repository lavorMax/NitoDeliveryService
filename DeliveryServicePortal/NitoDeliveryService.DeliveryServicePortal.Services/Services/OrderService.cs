using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
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

            var place = await _placeManagerHttpClient.Get(order.PlaceId, order.ClientId);
            var placeView = await _placeViewRepository.Read(order.PlaceViewId);

            var (addressLatitude, addressLongitude) = await GetCoordinates(order.Adress);

            var distance = GetDistance(addressLatitude, addressLongitude, placeView.Latitude, placeView.Longitude);

            var paymentConfig = place.PaymentConfigurations.OrderBy(i => i.MaxRange).FirstOrDefault(i => distance < i.MaxRange);

            order.DeliveryPrice = paymentConfig.Price;

            var result = await _orderRepository.Create(orderEntity);
            if (result != null)
            {
                throw new Exception("Error creating order");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUser(int userId, bool onlyActiveOrders = true)
        {
            var allOrdersEntities = await _orderRepository.GetOrdersByUser(userId, onlyActiveOrders);

            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(allOrdersEntities);

            return result;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByPlace(int clientId, int placeId, bool onlyActiveOrders = true)
        {
            var allOrdersEntities = await _orderRepository.GetOrdersByPlace(clientId, placeId, onlyActiveOrders);

            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(allOrdersEntities);

            return result;
        }

        public async Task UpdateOrderStatus(int orderId, OrderStatuses status)
        {
            var orderEntity = await _orderRepository.Read(orderId);

            orderEntity.OrderStatus = status;

            var result = await _orderRepository.Update(orderEntity);
            if (!result)
            {
                throw new Exception("Error updating order");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<OrderDTO> GetOrder(int orderId)
        {
            var order = await _orderRepository.ReadWithIncludes(orderId);

            var result = _mapper.Map<Order, OrderDTO>(order);

            return result;
        }

        private async Task<(double, double)> GetCoordinates(string address)
        {

            var geocoder = new ForwardGeocoder();
            var addressResponse = await geocoder.Geocode(new ForwardGeocodeRequest { queryString = address, BreakdownAddressElements = true });

            var addresDecoded = addressResponse.FirstOrDefault();
            if (addresDecoded == null)
            {
                throw new Exception("Error getting address");
            }

            return (addresDecoded.Latitude, addresDecoded.Longitude);
        }

        private double GetDistance(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            return Math.Sqrt(Math.Pow(Latitude1 - Latitude2, 2) + Math.Pow(Longitude1 - Longitude2, 2));
        }
    }
}
