using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientResponsibleController : ControllerBase
    {
        private readonly IClientResponsibleService _clientResponsibleService;

        public ClientResponsibleController(IClientResponsibleService clientResponsibleService)
        {
            _clientResponsibleService = clientResponsibleService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] ClientResponsibleDto responsible)
        {
            try
            {
                await _clientResponsibleService.AddClientResponsible(responsible);
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
                await _clientResponsibleService.RemoveClientResponsible(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("createphone")]
        public async Task<ActionResult> CreatePhone([FromBody] ClientPhoneDto phone)
        {
            try
            {
                await _clientResponsibleService.AddClientResponsiblePhone(phone);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("removephone/{id}")]
        public async Task<ActionResult> RemovePhone(int id)
        {
            try
            {
                await _clientResponsibleService.RemoveClientPhone(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
