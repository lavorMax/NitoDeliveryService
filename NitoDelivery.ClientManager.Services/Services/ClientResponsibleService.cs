using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Interfaces
{
    public class ClientResponsibleService : IClientResponsibleService
    {
        private readonly IClientResponsibleRepository _clientResponsibleRepository;
        private readonly IClientPhoneRepository _clientPhoneRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ClientResponsibleService(IClientResponsibleRepository clientResponsibleRepository, IClientPhoneRepository clientPhoneRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _clientResponsibleRepository = clientResponsibleRepository;
            _clientPhoneRepository = clientPhoneRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddClientResponsiblePhone(ClientPhoneDto clientPhone)
        {
            var phoneEntity = _mapper.Map<ClientPhoneDto, ClientPhone>(clientPhone);

            var result = await _clientPhoneRepository.Create(phoneEntity);
            if (result != null)
            {
                throw new Exception("Error creating client phone");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task AddClientResponsible(ClientResponsibleDto responsible)
        {
            var responsibleEntity = _mapper.Map<ClientResponsibleDto, ClientResponsible>(responsible);

            var result = await _clientResponsibleRepository.Create(responsibleEntity);
            if (result != null)
            {
                throw new Exception("Error creating client responsible");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveClientPhone(int id)
        {
            var result = await _clientPhoneRepository.Delete(id);
            if (!result)
            {
                throw new Exception("Error deleting client phone");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveClientResponsible(int id)
        {
            var result = await _clientResponsibleRepository.Delete(id);
            if (!result)
            {
                throw new Exception("Error removing client responsible");
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
