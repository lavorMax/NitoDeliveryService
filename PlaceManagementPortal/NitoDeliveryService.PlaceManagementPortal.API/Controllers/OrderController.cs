using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPut("close/{id}")]
        public async Task<ActionResult> Close(int id)
        {
            try
            {
                await _orderService.ChangeStatusToClosed(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("deliver/{id}")]
        public async Task<ActionResult> Deliver(int id)
        {
            try
            {
                await _orderService.ChangeStatusToDelivering(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("prepare/{id}")]
        public async Task<ActionResult> Prepare(int id)
        {
            try
            {
                await _orderService.ChangeStatusToPrepearing(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("getactive")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetActive()
        {
            try
            {
                var result = await _orderService.GetActiveOrders().ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id)
        {
            try
            {
                var result = await _orderService.GetOrder(id).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
