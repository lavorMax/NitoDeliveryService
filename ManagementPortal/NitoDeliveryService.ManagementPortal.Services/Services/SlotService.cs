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

                var result = await _slotRepository.Create(slot);

                if (result != null)
                {
                    throw new Exception("Error crating slots");
                }

                await _unitOfWork.SaveAsync();
            }
            
        }

        public async Task DeinitializeSlot(int id)
        {
            var slot = await _slotRepository.Read(id);

            if(slot == null)
            {
                throw new Exception("Error getting slot for deinitializing");
            }

            if (!slot.IsUsed)
            {
                return;
            }

            await _placeHttpClient.DeinitializeSlot(slot.ClientId, slot.Id);

            await _auth0cClient.DeleteUser(slot.ClientId.ToString()+slot.Id.ToString());

            slot.IsUsed = false;
            slot.Name = null;
            var result = await _slotRepository.Update(slot);

            if (!result)
            {
                throw new Exception("Error deinitializing slot");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<Auth0CredentialsResponse> GetCredentials(int slotId)
        {
            var slot = await _slotRepository.Read(slotId);

            if (slot == null)
            {
                throw new Exception("Error getting slot for deinitializing");
            }

            if (!slot.IsUsed)
            {
                throw new Exception("Slot is not in use");
            }

            var creds = await _auth0cClient.GetPassword(slot.ClientId.ToString() + slot.Id.ToString());

            return creds;
        }

        public async Task<Auth0CredentialsResponse> InitializeSlot(InitializeSlotRequest request)
        {
            var slot = await _slotRepository.Read(request.SlotId);

            if (slot == null)
            {
                throw new Exception("Error getting slot for initializing");
            }

            if (slot.IsUsed)
            {
                throw new Exception("Slot is in use");
            }

            await _placeHttpClient.InitializeSlot(slot.ClientId, request);

            var auth0Creds  = await _auth0cClient.CreateUser(slot.ClientId.ToString() + slot.Id.ToString(), slot.ClientId, slot.Id);

            slot.IsUsed = true;
            slot.Name = request.Name;
            var result = await _slotRepository.Update(slot);

            if (!result)
            {
                throw new Exception("Error initializing slot");
            }

            await _unitOfWork.SaveAsync();

            return auth0Creds;
        }

        public async Task RemoveSlots(int id)
        {
            await DeinitializeSlot(id);
            var result = await _slotRepository.Delete(id);

            if (!result)
            {
                throw new Exception("Error removing slots");
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
