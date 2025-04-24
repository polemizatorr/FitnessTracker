using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Interfaces;
using FitnessTracker.WebAPI.Services;
using FitnessTracker.WebAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
