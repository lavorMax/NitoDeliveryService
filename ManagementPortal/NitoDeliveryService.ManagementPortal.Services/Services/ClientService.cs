using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientRepository _clientRepository;
        private readonly IClientDbService _clientDbService;
        private readonly IMapper _mapper;

        public ClientService(IUnitOfWork unitOfWork, IClientRepository clientRepository, IClientDbService clientDbService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
            _clientDbService = clientDbService;
            _mapper = mapper;
        }

        public async Task CreateNewClient(ClientDto client)
        {

            var clientEntity = _mapper.Map<ClientDto, Client>(client);

            var result = await _clientRepository.Create(clientEntity).ConfigureAwait(false);
            if (result == null)
            {
                throw new Exception("Error crating client");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);

            await _clientDbService.CreateDb(result.Id).ConfigureAwait(false);
        }

        public async Task RemoveClient(int id)
        {
            await _clientDbService.RemoveDb(id).ConfigureAwait(false);

            var result = await _clientRepository.Delete(id).ConfigureAwait(false);
            if (!result)
            {
                throw new Exception("Error removing client");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ClientDto>> ReadAllClients()
        {
            var allClientsEntities = await _clientRepository.GetAll().ConfigureAwait(false);

            var result = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(allClientsEntities);

            return result;
        }

        public async Task<ClientDto> ReadFullClientById(int id)
        {
            var clientEntity = await _clientRepository.ReadWithIncludes(id).ConfigureAwait(false);

            var result = _mapper.Map<Client, ClientDto>(clientEntity);

            return result;
        }

        public async Task UpdateClientData(ClientDto client)
        {
            var clientEntity = _mapper.Map<ClientDto, Client>(client);

            var result = await _clientRepository.Update(clientEntity).ConfigureAwait(false);

            if (!result)
            {
                throw new Exception("Error updating client");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }
    }
}
