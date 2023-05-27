using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDelivery.ClientManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll()
        {
            try
            {
                var clients = await _clientService.ReadAllClients().ConfigureAwait(false);
                return Ok(clients);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ClientDto>> Get(int id)
        {
            try
            {
                var client = await _clientService.ReadFullClientById(id).ConfigureAwait(false);
                return Ok(client);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] ClientDto client)
        {
            try
            {
                await _clientService.CreateNewClient(client).ConfigureAwait(false);
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
                await _clientService.RemoveClient(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
