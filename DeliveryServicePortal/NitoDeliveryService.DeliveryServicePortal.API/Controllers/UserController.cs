using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("get/{login}")]
        public async Task<ActionResult<UserDTO>> Get(string login)
        {
            try
            {
                var result = await _userService.GetUser(login).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<int>> Create([FromBody] UserDTO request)
        {
            try
            {
                var id = await _userService.CreateUser(request).ConfigureAwait(false);
                return Ok(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
