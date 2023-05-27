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
    public class PaymentConfigurationService : IPaymentConfigurationService
    {
        private readonly IDeliveryServiceHttpClient _deliveryServiceHttpClient;
        private readonly IPlaceRepository _placeRepository;
        private readonly IPaymentConfigurationRepository _paymentConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentConfigurationService(IPaymentConfigurationRepository paymentConfigurationRepository,
            IPlaceRepository placeRepository,
            IDeliveryServiceHttpClient deliveryServiceHttpClient,
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _placeRepository = placeRepository;
            _deliveryServiceHttpClient = deliveryServiceHttpClient;
            _paymentConfigurationRepository = paymentConfigurationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewConfiguration(PaymentConfigurationDTO configuration)
        {
            var configurationEntity = _mapper.Map<PaymentConfigurationDTO, PaymentConfiguration>(configuration);

            var placeIdToUpdate = configurationEntity.PlaceId;

            var result = await _paymentConfigurationRepository.Create(configurationEntity).ConfigureAwait(false);

            if (result == null)
            {
                throw new Exception("Error creating payment configuration");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);

            await UpdatePlaceOnDeliveryPortal(placeIdToUpdate).ConfigureAwait(false);
        }

        public async Task RemoveConfiguration(int configurationId)
        {
            var configuration = await _paymentConfigurationRepository.Read(configurationId).ConfigureAwait(false);

            var placeIdToUpdate = configuration.PlaceId;

            var result = await _paymentConfigurationRepository.Delete(configurationId).ConfigureAwait(false);

            if (!result)
            {
                throw new Exception("Error removing payment configuration");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);

            await UpdatePlaceOnDeliveryPortal(placeIdToUpdate).ConfigureAwait(false);
        }

        private async Task UpdatePlaceOnDeliveryPortal(int placeId)
        {
            var placeToUpdate = await _placeRepository.ReadWithIncludes(placeId).ConfigureAwait(false);

            var placeDto = _mapper.Map<Place, PlaceDTO>(placeToUpdate);

            await _deliveryServiceHttpClient.UpdatePlace(placeDto).ConfigureAwait(false);
        }
    }
}
