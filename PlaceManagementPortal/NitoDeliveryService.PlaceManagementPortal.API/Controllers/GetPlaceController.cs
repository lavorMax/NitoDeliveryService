using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.API.Controllers
{
    [Authorize(Policy = "ClientCredentialsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class GetPlaceController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetPlaceController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        [HttpGet("get/{placeId}/{clientId}")]
        public async Task<ActionResult<PlaceDTO>> Get(int placeId, int clientId)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var factory = scope.ServiceProvider.GetRequiredService<IOverridingDbContextFactory<PlaceManagementDbContext>>();

                    factory.OverrideClientId(clientId);

                    var placeService = scope.ServiceProvider.GetRequiredService<IPlaceService>();
                    
                    var result = await placeService.GetPlace(placeId).ConfigureAwait(false);
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
