using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.DeliveryServicePortal.Services.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class PlaceViewService : IPlaceViewService
    {
        private readonly IPlaceManagementPortalHttpClient _placeManagerHttpClient;
        private readonly IPlaceViewRepository _placeViewRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceViewService(IPlaceManagementPortalHttpClient placeManagerHttpClient, IPlaceViewRepository placeViewRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _placeManagerHttpClient = placeManagerHttpClient;
            _placeViewRepository = placeViewRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePlaceView(PlaceDTO placeDto)
        {
            var PlaceView = new PlaceView()
            {
                PlaceId = placeDto.Id,
                Name = placeDto.Name,
                ClientId = placeDto.ClientId,
                Description = placeDto.Description,
                Deleted = false
            };

            var result = await _placeViewRepository.Create(PlaceView).ConfigureAwait(false);

            if (result == null)
            {
                throw new Exception("Error creating place");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task<PlaceDTO> Get(int placeId, int clientId)
        {
            var result = await _placeManagerHttpClient.Get(placeId, clientId).ConfigureAwait(false);

            if(result == null)
            {
                throw new Exception("Error getting place");
            }

            return result;
        }

        public async Task<IEnumerable<PlaceViewDTO>> GetAllPossibleToDeliver(string adress)
        {
            var (addressLatitude, addressLongitude) = await CoordinateGetter.GetCoordinates(adress).ConfigureAwait(false);

            var result = await _placeViewRepository.GetPossibleToDeliverPlaces(addressLatitude, addressLongitude).ConfigureAwait(false);

            var resultDto = _mapper.Map<IEnumerable<PlaceView>, IEnumerable<PlaceViewDTO>>(result);

            return resultDto;
        }

        public async Task UpdatePlaceView(PlaceDTO placeDto)
        {
            var entityToUpdate = await _placeViewRepository.ReadByPlaceAndClientId(placeDto.ClientId, placeDto.Id).ConfigureAwait(false);

            var (addressLatitude, addressLongitude) = await CoordinateGetter.GetCoordinates(placeDto.Address).ConfigureAwait(false);

            var PlaceView = new PlaceView()
            {
                Id = entityToUpdate.Id,
                PlaceId = placeDto.Id,
                Name = placeDto.Name,
                ClientId = placeDto.ClientId,
                Description = placeDto.Description,
                Adress = placeDto.Address,
                DeliveryRange = placeDto.PaymentConfigurations
                    .Select(pc => pc.MaxRange)
                    .OrderByDescending(i => i)
                    .FirstOrDefault(),

                Longitude = addressLongitude,
                Latitude = addressLatitude,
                Deleted = false
            };

            var result = await _placeViewRepository.Update(PlaceView).ConfigureAwait(false);

            if (!result)
            {
                throw new Exception("Error updating place");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task DeletePlaceView(int placeId, int clientId)
        {
            var entityToUpdate = await _placeViewRepository.ReadByPlaceAndClientId(clientId, placeId).ConfigureAwait(false);

            entityToUpdate.Deleted = true;

            var result = await _placeViewRepository.Update(entityToUpdate).ConfigureAwait(false);

            if (!result)
            {
                throw new Exception("Error updating place");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }
    }
}
