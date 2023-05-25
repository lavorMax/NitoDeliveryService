using AutoMapper;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Models.DTOs;

namespace NitoDelivery.ClientManager.API.Infrastructure
{
    public class Mapper : Profile
    {
        public void Init()
        {
            CreateMap<Slot, SlotDto>();
            CreateMap<SlotDto, Slot>();

            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto, Client>();
        }
    }
}
