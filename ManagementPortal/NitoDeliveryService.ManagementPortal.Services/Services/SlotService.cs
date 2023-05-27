using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Repositories.RepositoriesInterfaces;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Services
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlotRepository _slotRepository;
        private readonly IPlaceManagementPortalHttpClient _placeHttpClient;
        private readonly IAuth0ApiClient _auth0cClient;

        public SlotService(IUnitOfWork unitOfWork, ISlotRepository slotRepository, IPlaceManagementPortalHttpClient placeHttpClient, IAuth0ApiClient auth0cClient)
        {
            _unitOfWork = unitOfWork;
            _slotRepository = slotRepository;
            _placeHttpClient = placeHttpClient;
            _auth0cClient = auth0cClient;
        }

        public async Task CreateSlots(int clientId, int number = 1)
        {
            for(int i = 0; i < number; i++)
            {
                var slot = new Slot()
                {
                    IsUsed = false,
                    ClientId = clientId
                };

                var result = await _slotRepository.Create(slot).ConfigureAwait(false);

                if (result == null)
                {
                    throw new Exception("Error creating slots");
                }

                await _unitOfWork.SaveAsync().ConfigureAwait(false);
            }
            
        }

        public async Task DeinitializeSlot(int id)
        {
            var slot = await _slotRepository.Read(id).ConfigureAwait(false);

            if(slot == null)
            {
                throw new Exception("Error getting slot for deinitializing");
            }

            if (!slot.IsUsed)
            {
                return;
            }

            await _placeHttpClient.DeinitializeSlot(slot.ClientId, slot.Id).ConfigureAwait(false);

            await _auth0cClient.DeleteUser(slot.ManagerLogin).ConfigureAwait(false);

            slot.IsUsed = false;
            slot.Name = null;
            var result = await _slotRepository.Update(slot).ConfigureAwait(false);

            if (!result)
            {
                throw new Exception("Error deinitializing slot");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }

        public async Task<Auth0CredentialsResponse> GetCredentials(int slotId)
        {
            var slot = await _slotRepository.Read(slotId).ConfigureAwait(false);

            if (slot == null)
            {
                throw new Exception("Error getting slot for deinitializing");
            }

            if (!slot.IsUsed)
            {
                throw new Exception("Slot is not in use");
            }

            var creds = new Auth0CredentialsResponse()
            {
                auth0login = slot.ManagerLogin,
                auth0password = slot.ManagerPassword
            };

            return creds;
        }

        public async Task<Auth0CredentialsResponse> InitializeSlot(InitializeSlotRequest request)
        {
            var slot = await _slotRepository.Read(request.SlotId).ConfigureAwait(false);

            if (slot == null)
            {
                throw new Exception("Error getting slot for initializing");
            }

            if (slot.IsUsed)
            {
                throw new Exception("Slot is in use");
            }

            await _placeHttpClient.InitializeSlot(slot.ClientId, request).ConfigureAwait(false);

            var auth0Creds = await _auth0cClient.CreateUser(request.Email, slot.ClientId, slot.Id).ConfigureAwait(false);

            slot.ManagerPassword = auth0Creds.auth0password;
            slot.ManagerLogin = request.Email;
            slot.IsUsed = true;
            slot.Name = request.Name;
            var result = await _slotRepository.Update(slot).ConfigureAwait(false);

            if (!result)
            {
                throw new Exception("Error initializing slot");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);

            return auth0Creds;
        
        }

        public async Task RemoveSlots(int id)
        {
            await DeinitializeSlot(id).ConfigureAwait(false);
            var result = await _slotRepository.Delete(id).ConfigureAwait(false);

            if (!result)
            {
                throw new Exception("Error removing slots");
            }

            await _unitOfWork.SaveAsync().ConfigureAwait(false);
        }
    }
}
