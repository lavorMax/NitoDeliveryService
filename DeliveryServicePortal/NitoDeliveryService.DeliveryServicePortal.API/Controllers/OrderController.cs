﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitoDeliveryService.DeliveryServicePortal.API.Controllers
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

        [HttpGet("getall/{userId}/{onlyActive}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> Get(int userId, bool onlyActive = true)
        {
            try
            {
                var result = await _orderService.GetOrdersByUser(userId, onlyActive);
                return Ok(result);
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
                await _orderService.UpdateOrderStatus(orderId, status);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] OrderDTO request)
        {
            try
            {
                await _orderService.CreateOrder(request);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}