using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories;
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
        private readonly IAuth0Client _auth0Client;
        private readonly IPlaceRepository _placeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceService(IDeliveryServiceHttpClient deliveryServiceHttpClient, 
            IAuth0Client auth0Client, 
            IPlaceRepository placeRepository, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _deliveryServiceHttpClient = deliveryServiceHttpClient;
            _auth0Client = auth0Client;
            _placeRepository = placeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewPlace(InitializeSlotRequest place)
        {
            var placeEntity = new Place()
            {
                ClientId = place.ClientId,
                Name = place.Name,
                SlotId = place.SlotId
            };

            var result = await _placeRepository.Create(placeEntity);

            if (result == null)
            {
                throw new Exception("Error creating place");
            }

            await _unitOfWork.SaveAsync();

            var placeDTO = _mapper.Map<Place, PlaceDTO>(result);

            await _deliveryServiceHttpClient.CreatePlace(placeDTO);
        }

        public async Task<PlaceDTO> GetPlace(int placeId = -1)
        {
            if (placeId == -1)
            {
                var metadata = await _auth0Client.GetMetadata();
                placeId = metadata.PlaceId;
            }

            var entity = await _placeRepository.ReadWithIncludesBySlotId(placeId);

            if (entity == null)
            {
                throw new Exception("Error getting place");
            }

            var place = _mapper.Map<Place, PlaceDTO>(entity);

            return place;
        }

        public async Task RemovePlace(int slotId)
        {
            var place = await _placeRepository.ReadWithIncludesBySlotId(slotId);

            var result = await _placeRepository.DeleteBySlotId(slotId);

            if (!result)
            {
                throw new Exception("Error removing place");
            }

            await _deliveryServiceHttpClient.DeletePlace(place.Id, place.ClientId);

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

            await _deliveryServiceHttpClient.UpdatePlace(place);

            await _unitOfWork.SaveAsync();
        }
    }
}
