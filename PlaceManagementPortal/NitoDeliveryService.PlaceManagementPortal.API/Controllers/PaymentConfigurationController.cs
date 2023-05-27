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
    public class PaymentConfigurationController : ControllerBase
    {
        private readonly IPaymentConfigurationService _paymentConfigurationService;

        public PaymentConfigurationController(IPaymentConfigurationService paymentConfigurationService)
        {
            _paymentConfigurationService = paymentConfigurationService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] PaymentConfigurationDTO config)
        {
            try
            {
                await _paymentConfigurationService.CreateNewConfiguration(config).ConfigureAwait(false);
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
                await _paymentConfigurationService.RemoveConfiguration(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
