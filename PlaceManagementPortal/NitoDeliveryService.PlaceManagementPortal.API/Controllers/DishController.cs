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
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] DishDTO dish)
        {
            try
            {
                await _dishService.CreateNewDish(dish).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("remove/{Id}")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                await _dishService.RemoveDish(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
