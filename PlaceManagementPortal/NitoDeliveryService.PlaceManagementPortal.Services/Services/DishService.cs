using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DishService(IDishRepository dishRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewDish(DishDTO dish)
        {
            var dishEntity = _mapper.Map<DishDTO, Dish>(dish);

            var result = await _dishRepository.Create(dishEntity);

            if (result != null)
            {
                throw new Exception("Error creating dish");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveDish(int dishId)
        {
            var result = await _dishRepository.Delete(dishId);

            if (!result)
            {
                throw new Exception("Error removing dish");
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
