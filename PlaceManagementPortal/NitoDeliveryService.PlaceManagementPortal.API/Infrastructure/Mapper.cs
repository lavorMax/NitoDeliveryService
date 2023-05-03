using AutoMapper;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.Shared.Models.PlaceDTOs;

namespace NitoDeliveryService.PlaceManagementPortal.API.Infrastructure
{
    public class Mapper : Profile
    {
        public void Init()
        {
            CreateMap<Place, PlaceDTO>();
            CreateMap<PlaceDTO, Place>();

            CreateMap<Dish, DishDTO>();
            CreateMap<DishDTO, Dish>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<PlaceCategory, PlaceCategoryDTO>();
            CreateMap<PlaceCategoryDTO, PlaceCategory>();

            CreateMap<PaymentConfiguration, PaymentConfigurationDTO>();
            CreateMap<PaymentConfigurationDTO, PaymentConfiguration>();
        }
    }
}
