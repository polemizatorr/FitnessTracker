using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FitnessTracker.WebAPI.ApiResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using FitnessTracker.WebAPI.Interfaces;

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AerobicTrainingsController : ControllerBase
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly string ExportContentType = "application/json";
        private readonly string DefaultExportFileName = "Aerobic Trainings.json";
        private readonly IAerobicTrainingsService _service;

        public AerobicTrainingsController(TrainingsContext context, IHttpContextAccessor httpContextAccessor, IAerobicTrainingsService service)
        {
            _context = context;
            _http = httpContextAccessor; 
            _service = service;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<AerobicTrainingDto>>> GetAerobicTrainings()
        {
            var response = await _service.GetAerobicTrainings();

            return response;
        }

        [HttpGet("user/{username}")]
        public async Task<ApiResponse<IEnumerable<AerobicTrainingDto>>> GetAerobicTrainingsForUser(string username)
        {
            var response = await _service.GetAerobicTrainingsForUser(username);

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<AerobicTrainingDto>> GetAerobicTraining(Guid id)
        {
            var response = await _service.GetAerobicTraining(id);

            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<AerobicTrainingDto>> PutAerobicTraining(Guid id, AerobicTrainingDto aerobicTraining)
        {
            var response = await _service.PutAerobicTraining(id, aerobicTraining);

            return response;
        }

        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<AerobicTrainingDto>> PostAerobicTraining(AerobicTrainingDto aerobicTraining)
        {
            var response = await _service.PostAerobicTraining(aerobicTraining);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<AerobicTrainingDto>> DeleteAerobicTraining(Guid id)
        {
            var response = await _service.DeleteAerobicTraining(id);

            return response;
        }
        [HttpGet("export")]
        public async Task<IActionResult> exportTrainings()
        {
            var username = _http.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (username == null)
            {
                var errorResponse = new ApiResponse<string>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrorMessage = "Failed to find user",
                    StatusCode = 400
                };

                return BadRequest(errorResponse);
            }
            var userId = _context.Users.First(u => u.UserName == username).UserId;

            var trainings = await _context.AerobicTrainings.Where(t => t.UserId == userId).ToListAsync();

            if (!trainings.Any()) 
            {
                var errorResponse = new ApiResponse<string>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrorMessage = "No trainings found to export",
                    StatusCode = 400
                };

                return BadRequest(errorResponse);
            }

            var trainingsData = trainings.Select(t => t.ToDto()).ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var exportData = JsonSerializer.Serialize(trainingsData, options);
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(exportData);

            Response.Headers.Append("Content-Disposition", "attachment; filename=Aerobic Trainings.json");
            return File(fileBytes, ExportContentType, DefaultExportFileName);
        }
    }
}
