using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public async Task CreateOrder(OrderDTO order)
        {
            var orderEntity = _mapper.Map<OrderDTO, Order>(order);

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
    }
}
