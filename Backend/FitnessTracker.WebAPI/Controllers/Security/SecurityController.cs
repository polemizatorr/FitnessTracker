using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Controllers.Security
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _service;
        public SecurityController(ISecurityService service) 
        { 
            _service = service;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> Register(RegisterUserDTO user)
        {
            var response = await _service.Register(user);

            return response;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ApiResponse<string> Login(UserDto user)
        {
            var response = _service.Login(user);

            return response;
        }
    }
}
