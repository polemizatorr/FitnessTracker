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

namespace FitnessTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrengthTrainingsController : ControllerBase
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;

        private readonly string ExportContentType = "application/json";
        private readonly string DefaultExportFileName = "Strength Trainings.json";

        public StrengthTrainingsController(TrainingsContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _http = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<StrengthTrainingResponse>>> GetStrengthTrainings()
        {
          if (_context.StrenghtTrainings == null)
          {
                return new ApiResponse<IEnumerable<StrengthTrainingResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = "Trainings not defined",
                    StatusCode = 404
                };
            }
          var strengthTrainings = await _context.StrenghtTrainings.Include(st => st.Sets)
                .Select(st => new StrengthTrainingResponse
                {
                    StrenghtTrainingId = st.StrenghtTrainingId,
                    Sets = st.Sets,
                    TrainingDate = st.TrainingDate,
                    TrainingName = st.TrainingName,
                })
                
                .ToListAsync();

          return new ApiResponse<IEnumerable<StrengthTrainingResponse>>
          {
              IsSuccess = true,
              StatusCode = 200,
              Data = strengthTrainings,
          };
        }

        [HttpGet("user/{username}")]
        public async Task<ApiResponse<IEnumerable<StrengthTrainingResponse>>> GetStrengthTraining(string username)
        {
            if (_context.StrenghtTrainings == null)
            {
                return new ApiResponse<IEnumerable<StrengthTrainingResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = "Trainings not defined",
                    StatusCode = 404
                };
            }

            var userId = _context.Users?.FirstOrDefault(u => u.UserName == username)?.UserId;

            var strenghtTraining = await _context.StrenghtTrainings
                .Where(st => st.UserId == userId)
                .Include(st => st.Sets)
                .Select(st => new StrengthTrainingResponse
                {
                    StrenghtTrainingId = st.StrenghtTrainingId,
                    Sets = st.Sets,
                    TrainingDate = st.TrainingDate,
                    TrainingName = st.TrainingName,
                })
                .ToListAsync();

            if (strenghtTraining == null)
            {
                return new ApiResponse<IEnumerable<StrengthTrainingResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = "Training not found",
                    StatusCode = 404
                };
            }

            return new ApiResponse<IEnumerable<StrengthTrainingResponse>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = strenghtTraining,
            };
        }

        [HttpGet("{id}", Name = "GetStrengthTraining")]
        public async Task<ApiResponse<StrengthTraining>> GetStrengthTraining(Guid id)
        {
          if (_context.StrenghtTrainings == null)
          {
                return new ApiResponse<StrengthTraining>
                {
                    IsSuccess = false,
                    ErrorMessage = "Trainings not defined",
                    StatusCode = 404
                };
            }
            var strenghtTraining = await _context.StrenghtTrainings.Include(st => st.Sets).FirstOrDefaultAsync(st => st.StrenghtTrainingId == id);

            if (strenghtTraining == null)
            {
                return new ApiResponse<StrengthTraining>
                {
                    IsSuccess = false,
                    ErrorMessage = "Trainings not defined",
                    StatusCode = 404
                };
            }

            return new ApiResponse<StrengthTraining>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = strenghtTraining,
            };
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<StrengthTraining>> PutStrenghtTraining(Guid id, StrengthTrainingDTO strenghtTrainingData)
        {
            var editStrenghtTraining = _context.StrenghtTrainings.Include(st => st.Sets).SingleOrDefault(st => st.StrenghtTrainingId == id);

            if (editStrenghtTraining == null)
            {
                return new ApiResponse<StrengthTraining>
                {
                    IsSuccess = false,
                    ErrorMessage = "Trainings not found",
                    StatusCode = 404
                };
            }
            try
            {
                editStrenghtTraining.Sets.Clear();

                foreach (var data in strenghtTrainingData.Sets)
                {
                    var set = new Set(editStrenghtTraining.StrenghtTrainingId, data.RepetitionsNumber, data.ExerciseName!, data.ExhaustionLevel, data.Weight);
                    editStrenghtTraining.Sets.Add(set);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StrenghtTrainingExists(id))
                {
                    return new ApiResponse<StrengthTraining>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Trainings not found",
                        StatusCode = 404
                    };
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<StrengthTraining>
            {
                IsSuccess = true,
                StatusCode = 204,
            };
        }

        [HttpPost]
        public async Task<ApiResponse<StrengthTraining>> PostStrenghtTraining(StrengthTrainingDTO strenghtTrainingData)
        {
          if (_context.StrenghtTrainings == null)
          {
                return new ApiResponse<StrengthTraining>
                {
                    IsSuccess = false,
                    ErrorMessage = "Trainings not found",
                    StatusCode = 404
                };
            }

            var username = _http.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = _context.Users.First(u => u.UserName == username).UserId;

            if (userId == Guid.Empty)
            {
                return new ApiResponse<StrengthTraining>
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found",
                    StatusCode = 404
                };
            }

            var strenghtTraining = new StrengthTraining(userId, strenghtTrainingData.TrainingName!, strenghtTrainingData.TrainingDate);

            foreach (var data in strenghtTrainingData.Sets)
            {
                var set = new Set(strenghtTraining.StrenghtTrainingId, data.RepetitionsNumber, data.ExerciseName!, data.ExhaustionLevel, data.Weight);
                strenghtTraining.Sets.Add(set);
            }


            _context.StrenghtTrainings.Add(strenghtTraining);
            await _context.SaveChangesAsync();

            return new ApiResponse<StrengthTraining>
            {
                IsSuccess = true,
                StatusCode = 205,
                Data = strenghtTraining,
            };
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<StrengthTraining>> DeleteStrenghtTraining(Guid id)
        {
            if (_context.StrenghtTrainings == null)
            {
                return new ApiResponse<StrengthTraining>
                {
                    IsSuccess = false,
                    ErrorMessage = "Trainings not defined",
                    StatusCode = 404
                };
            }
            var strenghtTraining = await _context.StrenghtTrainings.FindAsync(id);
            if (strenghtTraining == null)
            {
                return new ApiResponse<StrengthTraining>
                {
                    IsSuccess = false,
                    ErrorMessage = "Training not found",
                    StatusCode = 404
                };
            }

            _context.StrenghtTrainings.Remove(strenghtTraining);
            await _context.SaveChangesAsync();

            return new ApiResponse<StrengthTraining>
            {
                IsSuccess = true,
                StatusCode = 204
            };
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

            Response.Headers.Add("Content-Disposition", "attachment; filename=Aerobic Trainings.json");
            return File(fileBytes, ExportContentType, DefaultExportFileName);
        }

        private bool StrenghtTrainingExists(Guid id)
        {
            return (_context.StrenghtTrainings?.Any(e => e.StrenghtTrainingId == id)).GetValueOrDefault();
        }
    }
}
