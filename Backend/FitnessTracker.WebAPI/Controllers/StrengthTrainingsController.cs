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
using System.Security.Claims;
using FitnessTracker.WebAPI.ApiResponse;
using System.Text.Json;
using FitnessTracker.WebAPI.Interfaces;

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrengthTrainingsController : ControllerBase
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly IStrengthTrainingsService _service;

        private readonly string ExportContentType = "application/json";
        private readonly string DefaultExportFileName = "Strength Trainings.json";

        public StrengthTrainingsController(TrainingsContext context, IHttpContextAccessor httpContextAccessor, IStrengthTrainingsService service)
        {
            _context = context;
            _http = httpContextAccessor;
            _service = service;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<StrengthTrainingResponse>>> GetStrengthTrainings()
        {
            var response = await _service.GetStrengthTrainings();

            return response;
        }

        [HttpGet("user/{username}")]
        public async Task<ApiResponse<IEnumerable<StrengthTrainingResponse>>> GetStrengthTraining(string username)
        {
            var response = await _service.GetStrengthTraining(username);

            return response;
        }

        [HttpGet("{id}", Name = "GetStrengthTraining")]
        public async Task<ApiResponse<StrengthTraining>> GetStrengthTraining(Guid id)
        {
            var response = await _service.GetStrengthTraining(id);

            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<StrengthTraining>> PutStrenghtTraining(Guid id, StrengthTrainingDTO strenghtTrainingData)
        {
            var response = await _service.PutStrenghtTraining(id, strenghtTrainingData);

            return response;
        }

        [HttpPost]
        public async Task<ApiResponse<StrengthTraining>> PostStrenghtTraining(StrengthTrainingDTO strenghtTrainingData)
        {
            var response = await _service.PostStrenghtTraining(strenghtTrainingData);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<StrengthTraining>> DeleteStrenghtTraining(Guid id)
        {
            var response = await _service.DeleteStrenghtTraining(id);

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

            var trainings = await _context.StrenghtTrainings.Include(t => t.Sets).Where(t => t.UserId == userId).ToListAsync();

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
            try
            {
                var x = trainings.Select(t => t.ToDto()).ToList();
            }
            catch (Exception ex)
            {
                return BadRequest();
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

        private bool StrenghtTrainingExists(Guid id)
        {
            return (_context.StrenghtTrainings?.Any(e => e.StrenghtTrainingId == id)).GetValueOrDefault();
        }
    }
}
