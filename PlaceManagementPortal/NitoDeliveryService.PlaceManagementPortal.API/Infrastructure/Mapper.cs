using AutoMapper;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.Shared.Models.PlaceDTOs;

namespace NitoDeliveryService.PlaceManagementPortal.API.Infrastructure
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Place, PlaceDTO>();
            CreateMap<PlaceDTO, Place>().ForMember(dest => dest.SlotId, opt => opt.Ignore());

            CreateMap<Dish, DishDTO>();
            CreateMap<DishDTO, Dish>();

            CreateMap<PaymentConfiguration, PaymentConfigurationDTO>();
            CreateMap<PaymentConfigurationDTO, PaymentConfiguration>();
        }
    }
}
