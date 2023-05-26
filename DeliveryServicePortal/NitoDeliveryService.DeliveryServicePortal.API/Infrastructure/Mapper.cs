using AutoMapper;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.Shared.Models.PlaceDTOs;

namespace NitoDeliveryService.DeliveryServicePortal.API.Infrastructure
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDTO>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<UserDTO, User>();

            CreateMap<PlaceView, PlaceViewDTO>();
            CreateMap<PlaceViewDTO, PlaceView>();

            CreateMap<DishOrder, DishOrderDTO>();
            CreateMap<DishOrderDTO, DishOrder>();

            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();
        }
    }
}
