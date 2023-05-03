using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
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
            var (addressLatitude, addressLongitude) = await GetCoordinates(placeDto.Address);

            var categories = _mapper.Map<IEnumerable<PlaceCategoryDTO>, IEnumerable<CategoryView>>(placeDto.PlaceCategories);

            var PlaceView = new PlaceView()
            {
                PlaceId = placeDto.Id,
                Name = placeDto.Name,
                ClientId = placeDto.ClientId,
                Description = placeDto.Description,
                Adress = placeDto.Address,
                DeliveryRange = placeDto.PaymentConfigurations.Select(pc => pc.MaxRange).OrderBy(i => i).First(),
                Longitude = addressLongitude,
                Latitude = addressLatitude,

                Categories = categories
            };

            var result = await _placeViewRepository.Create(PlaceView);

            if (result == null)
            {
                throw new Exception("Error creating place");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<PlaceDTO> Get(int placeId, int clientId)
        {
            var result = await _placeManagerHttpClient.Get(placeId, clientId);

            if(result == null)
            {
                throw new Exception("Error getting place");
            }

            return result;
        }

        public async Task<IEnumerable<PlaceViewDTO>> GetAllPossibleToDeliver(string adress)
        {
            var (addressLatitude, addressLongitude) = await GetCoordinates(adress);

            var result = await _placeViewRepository.GetPossibleToDeliverPlaces(addressLatitude, addressLongitude);

            var resultDto = _mapper.Map<IEnumerable<PlaceView>, IEnumerable<PlaceViewDTO>>(result);

            return resultDto;
        }

        public async Task<IEnumerable<PlaceViewDTO>> Search(string adress, string keys)
        {
            var (addressLatitude, addressLongitude) = await GetCoordinates(adress);

            var result = await _placeViewRepository.SearchByName(addressLatitude, addressLongitude, keys.Split(" "));

            var resultDto = _mapper.Map<IEnumerable<PlaceView>, IEnumerable<PlaceViewDTO>>(result);

            return resultDto;
        }

        public async Task<IEnumerable<PlaceViewDTO>> SearchByCategory(string adress, PlaceCategories category)
        {
            var (addressLatitude, addressLongitude) = await GetCoordinates(adress);

            var result = await _placeViewRepository.SearchByCategory(addressLatitude, addressLongitude, category);

            var resultDto = _mapper.Map<IEnumerable<PlaceView>, IEnumerable<PlaceViewDTO>>(result);

            return resultDto;
        }

        public async Task UpdatePlaceView(PlaceDTO placeDto)
        {
            var entityToUpdate = _placeViewRepository.ReadByPlaceAndClientId(placeDto.ClientId, placeDto.Id);

            var (addressLatitude, addressLongitude) = await GetCoordinates(placeDto.Address);

            var categories = _mapper.Map<IEnumerable<PlaceCategoryDTO>, IEnumerable<CategoryView>>(placeDto.PlaceCategories);

            var PlaceView = new PlaceView()
            {
                Id = entityToUpdate.Id,
                PlaceId = placeDto.Id,
                Name = placeDto.Name,
                ClientId = placeDto.ClientId,
                Description = placeDto.Description,
                Adress = placeDto.Address,
                DeliveryRange = placeDto.PaymentConfigurations.Select(pc => pc.MaxRange).OrderBy(i => i).First(),
                Longitude = addressLongitude,
                Latitude = addressLatitude,

                Categories = categories
            };

            var result = await _placeViewRepository.Update(PlaceView);

            if (!result)
            {
                throw new Exception("Error updating place");
            }

            await _unitOfWork.SaveAsync();
        }

        private async Task<(double, double)> GetCoordinates(string address)
        {

            var geocoder = new ForwardGeocoder();
            var addressResponse = await geocoder.Geocode(new ForwardGeocodeRequest { queryString = address, BreakdownAddressElements = true });

            var addresDecoded = addressResponse.FirstOrDefault();
            if (addresDecoded == null)
            {
                throw new Exception("Error getting address");
            }

            return (addresDecoded.Latitude, addresDecoded.Longitude);
        }
    }
}
