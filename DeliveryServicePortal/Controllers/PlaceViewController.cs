using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
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
        public async Task<ActionResult<PlaceViewDTO>> Get(int placeId, int clientId)
        {
            try
            {
                var result = await _placeService.Get(clientId, placeId);
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
                var result = await _placeService.GetAllPossibleToDeliver(adress);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("search/{adress}/{keys}")]
        public async Task<ActionResult<IEnumerable<PlaceViewDTO>>> Search(string adress, string keys)
        {
            try
            {
                var result = await _placeService.Search(adress, keys);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("searchCategory/{adress}/{category}")]
        public async Task<ActionResult<IEnumerable<PlaceViewDTO>>> SearchByCategory(string adress, PlaceCategories category)
        {
            try
            {
                var result = await _placeService.SearchByCategory(adress, category);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
