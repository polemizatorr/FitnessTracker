using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Interfaces;
using FitnessTracker.WebAPI.Services;
using FitnessTracker.WebAPI.ApiResponse;

namespace FitnessTracker.WebAPI.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;

        public UsersController(IUsersService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<User>> GetUser(Guid id)
        {
            var response = await _service.GetUser(id);

            return response;
        }

        [HttpGet]
        public ApiResponse<IEnumerable<User>> GetUsers()
        {
            var response = _service.GetUsers();

            return response;
        }

        [HttpPost]
        public async Task<ApiResponse<User>> PostUser(CreateUserDTO user)
        {
            var response = await _service.PostUser(user);

            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<User>> PutUser(Guid id, User user)
        {
           var response = await _service.PutUser(id, user);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<User>> DeleteUser(Guid id)
        {
            var response = await _service.DeleteUser(id);

            return response;
        }

        [HttpDelete("ByUsername/{username}")]
        public async Task<ApiResponse<User>> DeleteUserByUsername(string username)
        {
            var response = await _service.DeleteUserByUsername(username);

            return response;
        }
    }
}
