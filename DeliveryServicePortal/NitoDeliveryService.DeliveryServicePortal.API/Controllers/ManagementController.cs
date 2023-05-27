using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.DeliveryServicePortal.API.Controllers
{
    [Authorize(Policy = "ClientCredentialsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPlaceViewService _placeService;

        public ManagementController(IOrderService orderService, IPlaceViewService placeService)
        {
            _orderService = orderService;
            _placeService = placeService;
        }

        [HttpGet("getall/{placeId}/{clientId}/{onlyActive}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> Get(int placeId, int clientId, bool onlyActive = true)
        {
            try
            {
                var result = await _orderService.GetOrdersByPlace(clientId, placeId, onlyActive).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] PlaceDTO request)
        {
            try
            {
                await _placeService.CreatePlaceView(request).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] PlaceDTO request)
        {
            try
            {
                await _placeService.UpdatePlaceView(request).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("delete/{placeId}/{clientId}")]
        public async Task<ActionResult> Remove(int placeId, int clientId)
        {
            try
            {
                await _placeService.DeletePlaceView(placeId, clientId).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("status/{orderId}")]
        public async Task<ActionResult> Status(int orderId, [FromBody] OrderStatuses status)
        {
            try
            {
                await _orderService.UpdateOrderStatus(orderId, status).ConfigureAwait(false);
                return Ok();
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
