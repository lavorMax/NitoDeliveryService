using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.DTOs;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IDeliveryServiceHttpClient _deliveryServiceHttpClient;
        private readonly ITokenParser _tokenParser;
        private readonly IPlaceRepository _placeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceService(IDeliveryServiceHttpClient deliveryServiceHttpClient, ITokenParser tokenParser, IPlaceRepository placeRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _deliveryServiceHttpClient = deliveryServiceHttpClient;
            _tokenParser = tokenParser;
            _placeRepository = placeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewPlace(InitializeSlotRequest place)
        {
            var userMetadata = _tokenParser.GetMetadata();

            var placeEntity = new Place()
            {
                ClientId = userMetadata.ClientId,
                Name = place.Name,
                Description = place.Description,
                SlotId = place.SlotId
            };

            var result = await _placeRepository.Create(placeEntity);

            if (result != null)
            {
                throw new Exception("Error creating place");
            }

            var placeDTO = _mapper.Map<Place, PlaceDTO>(result);
            var deliveryServiceResult = await _deliveryServiceHttpClient.CreatePlace(placeDTO);

            if (!deliveryServiceResult)
            {
                throw new Exception("Error creating place in deliveryService API");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<PlaceDTO> GetPlace()
        {
            var userMetadata = _tokenParser.GetMetadata();

            var entity = await _placeRepository.ReadWithIncludes(userMetadata.PlaceId);

            if (entity == null)
            {
                throw new Exception("Error getting place");
            }

            var place = _mapper.Map<Place, PlaceDTO>(entity);

            return place;
        }

        public async Task RemovePlace(int slotId)
        {
            var result = await _placeRepository.DeleteBySlotId(slotId);

            if (!result)
            {
                throw new Exception("Error removing place");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdatePlace(PlaceDTO place)
        {
            var placeEntity = _mapper.Map<PlaceDTO, Place>(place);

            var result = await _placeRepository.Update(placeEntity);

            if (!result)
            {
                throw new Exception("Error updatng place");
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
