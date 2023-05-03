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
        private readonly IPaymentConfigurationRepository _paymentConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentConfigurationService(IPaymentConfigurationRepository paymentConfigurationRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _paymentConfigurationRepository = paymentConfigurationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewConfiguration(PaymentConfigurationDTO configuration)
        {
            var configurattionEntity = _mapper.Map<PaymentConfigurationDTO, PaymentConfiguration>(configuration);

            var result = await _paymentConfigurationRepository.Create(configurattionEntity);

            if (result != null)
            {
                throw new Exception("Error creating payment configuration");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveConfiguration(int configurationId)
        {
            var result = await _paymentConfigurationRepository.Delete(configurationId);

            if (!result)
            {
                throw new Exception("Error removing payment configuration");
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
