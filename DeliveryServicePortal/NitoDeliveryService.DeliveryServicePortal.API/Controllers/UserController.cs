﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.DeliveryServicePortal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult<UserDTO>> Get(int userId)
        {
            try
            {
                var result = await _userService.GetUser(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Status([FromBody] UserDTO user)
        {
            try
            {
                await _userService.UpdateUser(user);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] UserDTO request)
        {
            try
            {
                await _userService.CreateUser(request);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}