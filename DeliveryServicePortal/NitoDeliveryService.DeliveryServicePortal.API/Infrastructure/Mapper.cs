using AutoMapper;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.Shared.Models.PlaceDTOs;

namespace NitoDeliveryService.DeliveryServicePortal.API.Infrastructure
{
    public class Mapper : Profile
    {
        public void Init()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<PlaceView, PlaceViewDTO>();
            CreateMap<PlaceViewDTO, PlaceView>();

            CreateMap<CategoryView, PlaceCategoryDTO>();
            CreateMap<PlaceCategoryDTO, CategoryView>();

            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();
        }
    }
}
