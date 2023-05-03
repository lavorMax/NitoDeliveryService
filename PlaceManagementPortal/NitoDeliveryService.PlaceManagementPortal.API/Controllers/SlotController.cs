using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.API.Controllers
{
    [Authorize(Policy = "ClientCredentialsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SlotController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        [HttpPost("initialize/{clientId}")]
        public async Task<ActionResult> Create(int clientId, [FromBody] InitializeSlotRequest request)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var factory = scope.ServiceProvider.GetRequiredService<PlaceManagementDbContextFactory>();
                    factory.OverrideClientId(clientId);

                    var placeService = scope.ServiceProvider.GetRequiredService<IPlaceService>();
                    await placeService.CreateNewPlace(request);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("deinitialize/{clientId}/{slotId}")]
        public async Task<ActionResult> Remove(int clientId, int slotId)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var factory = scope.ServiceProvider.GetRequiredService<PlaceManagementDbContextFactory>();
                    factory.OverrideClientId(clientId);

                    var placeService = scope.ServiceProvider.GetRequiredService<IPlaceService>();
                    await placeService.RemovePlace(slotId);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
