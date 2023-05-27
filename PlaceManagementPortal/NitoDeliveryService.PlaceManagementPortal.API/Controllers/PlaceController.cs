using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] PlaceDTO request)
        {
            try
            {
                await _placeService.UpdatePlace(request).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get")]
        public async Task<ActionResult<PlaceDTO>> Get()
        {
            try
            {
                var result = await _placeService.GetPlace().ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
