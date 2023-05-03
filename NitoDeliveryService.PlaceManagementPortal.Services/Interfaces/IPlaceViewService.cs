﻿using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IPlaceViewService
    {
        Task CreatePlaceView(PlaceDTO placeDto);
        Task UpdatePlaceView(PlaceDTO placeDto);
        Task<IEnumerable<PlaceViewDTO>> GetAllPossibleToDeliver(string adress);
        Task<IEnumerable<PlaceViewDTO>> Search(string adress, string keys);
        Task<IEnumerable<PlaceViewDTO>> SearchByCategory(string adress, PlaceCategories category);
        Task<PlaceDTO> Get(int placeId, int clientId);
    }
}
