using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.DTOs;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Runtime.InteropServices;
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

            var result = await _placeRepository.Create(placeEntity).ConfigureAwait(false);

            if (result == null)
            {
                throw new ExternalException("Error creating place");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);

            var placeDTO = _mapper.Map<Place, PlaceDTO>(result);

            await _deliveryServiceHttpClient.CreatePlace(placeDTO).ConfigureAwait(false);
        }

        public async Task<PlaceDTO> GetPlace(int placeId)
        {
            var entity = await _placeRepository.ReadWithIncludes(placeId).ConfigureAwait(false);

            if (entity == null)
            {
                throw new ExternalException("Error getting place");
            }

            var place = _mapper.Map<Place, PlaceDTO>(entity);

            return place;
        }

        public async Task<PlaceDTO> GetPlace()
        {
            var metadata = await _auth0Client.GetMetadata().ConfigureAwait(false);
            var slotId = metadata.PlaceId;

            var entity = await _placeRepository.ReadWithIncludesBySlotId(slotId).ConfigureAwait(false);

            if (entity == null)
            {
                throw new ExternalException("Error getting place");
            }

            var place = _mapper.Map<Place, PlaceDTO>(entity);

            return place;
        }

        public async Task RemovePlace(int slotId)
        {
            var place = await _placeRepository.ReadWithIncludesBySlotId(slotId).ConfigureAwait(false);

            var result = await _placeRepository.DeleteBySlotId(slotId).ConfigureAwait(false);

            if (!result)
            {
                throw new ExternalException("Error removing place");
            }

            await _deliveryServiceHttpClient.DeletePlace(place.Id, place.ClientId).ConfigureAwait(false);

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task UpdatePlace(PlaceDTO place)
        {
            var placeEntity = _mapper.Map<PlaceDTO, Place>(place);

            var result = await _placeRepository.Update(placeEntity).ConfigureAwait(false);

            if (!result)
            {
                throw new ExternalException("Error updatng place");
            }

            await _deliveryServiceHttpClient.UpdatePlace(place).ConfigureAwait(false);

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }
    }
}
