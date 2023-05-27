using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Runtime.InteropServices;
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

            var result = await _dishRepository.Create(dishEntity).ConfigureAwait(false);

            if (result == null)
            {
                throw new ExternalException("Error creating dish");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task RemoveDish(int dishId)
        {
            var result = await _dishRepository.Delete(dishId).ConfigureAwait(false);

            if (!result)
            {
                throw new ExternalException("Error removing dish");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }
    }
}
