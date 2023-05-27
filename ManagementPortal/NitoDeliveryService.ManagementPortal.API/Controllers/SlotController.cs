using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace NitoDelivery.ClientManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpPost("create/{clientId}/{number}")]
        public async Task<ActionResult> Create(int clientId, int number = 1)
        {
            try
            {
                await _slotService.CreateSlots(clientId, number).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                await _slotService.RemoveSlots(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("initialize")]
        public async Task<ActionResult<Auth0CredentialsResponse>> Initialize([FromBody] InitializeSlotRequest request)
        {
            try
            {
                var credentials = await _slotService.InitializeSlot(request).ConfigureAwait(false);
                return Ok(credentials);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("deinitialize/{id}")]
        public async Task<ActionResult> Deinitialize(int id)
        {
            try
            {
                await _slotService.DeinitializeSlot(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("credentials/{id}")]
        public async Task<ActionResult<Auth0CredentialsResponse>> GetCredentials(int id)
        {
            try
            {
                var credentials = await _slotService.GetCredentials(id).ConfigureAwait(false);
                return Ok(credentials);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
