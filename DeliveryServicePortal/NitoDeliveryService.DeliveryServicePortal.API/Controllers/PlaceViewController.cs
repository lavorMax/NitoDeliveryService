using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.DeliveryServicePortal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceViewController : ControllerBase
    {
        private readonly IPlaceViewService _placeService;

        public PlaceViewController(IPlaceViewService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet("get/{placeId}/{clientId}")]
        public async Task<ActionResult<PlaceDTO>> Get(int placeId, int clientId)
        {
            try
            {
                var result = await _placeService.Get(placeId, clientId).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("getall/{adress}")]
        public async Task<ActionResult<IEnumerable<PlaceViewDTO>>> Get(string adress)
        {
            try
            {
                var result = await _placeService.GetAllPossibleToDeliver(adress).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
