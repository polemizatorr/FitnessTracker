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

        public AerobicTrainingsController(TrainingsContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _http = httpContextAccessor; 
        }

        // GET: api/AerobicTrainings
        [HttpGet]
        // [Authorize]
        public async Task<ApiResponse<IEnumerable<AerobicTrainingDto>>> GetAerobicTrainings()
        {
          if (_context.AerobicTrainings == null)
          {
                return new ApiResponse<IEnumerable<AerobicTrainingDto>>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "No Aerobic Trainings Data Found",                   
                };
          }

            var aerobicTrainings = await _context.AerobicTrainings.Select(at => new AerobicTrainingDto
            {
                ActivityDurationMinutes = at.ActivityDurationMinutes,
                ActivityDate = at.ActivityDate,
                ActivityType = at.ActivityType,
                CalorieBurnt = at.CalorieBurnt,
                AerobicTrainingId = at.AerobicTrainingId,
            }).ToListAsync();

            return new ApiResponse<IEnumerable<AerobicTrainingDto>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = aerobicTrainings,
            };
        }

        // GET: api/AerobicTrainings/user/username
        [HttpGet("user/{username}")]
        // [Authorize]
        public async Task<ApiResponse<IEnumerable<AerobicTrainingDto>>> GetAerobicTrainingsForUser(string username)
        {
            if (_context.AerobicTrainings == null)
            {
                return new ApiResponse<IEnumerable<AerobicTrainingDto>>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "No Aerobic Trainings Data Found for this user",
                };
            }

            var userId = _context.Users?.FirstOrDefault(u => u.UserName == username)?.UserId;
            var aerobicTrainings = await _context.AerobicTrainings.Where(at => at.UserId == userId).Select(at => new AerobicTrainingDto
            {
                ActivityDurationMinutes = at.ActivityDurationMinutes,
                ActivityDate = at.ActivityDate,
                ActivityType = at.ActivityType,
                CalorieBurnt = at.CalorieBurnt,
                AerobicTrainingId = at.AerobicTrainingId,
            }).ToListAsync();

            return new ApiResponse<IEnumerable<AerobicTrainingDto>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = aerobicTrainings,
            };
        }

        // GET: api/AerobicTrainings/5
        [HttpGet("{id}")]
        public async Task<ApiResponse<AerobicTrainingDto>> GetAerobicTraining(Guid id)
        {
          if (_context.AerobicTrainings == null)
          {
                return new ApiResponse<AerobicTrainingDto>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "Aerobic Trainings Not Defined",
                };
            }
            var aerobicTraining = await _context.AerobicTrainings.FindAsync(id);

            if (aerobicTraining == null)
            {
                return new ApiResponse<AerobicTrainingDto>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "Aerobic Training Not Found",
                };
            }

            return new ApiResponse<AerobicTrainingDto>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = new AerobicTrainingDto
                {
                    ActivityDurationMinutes = aerobicTraining.ActivityDurationMinutes,
                    ActivityDate = aerobicTraining.ActivityDate,
                    ActivityType = aerobicTraining.ActivityType,
                    CalorieBurnt = aerobicTraining.CalorieBurnt,
                    AerobicTrainingId = aerobicTraining.AerobicTrainingId,
                }
            };
        }

        // PUT: api/AerobicTrainings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ApiResponse<AerobicTrainingDto>> PutAerobicTraining(Guid id, AerobicTrainingDto aerobicTraining)
        {
            var editTraining = _context.AerobicTrainings.Find(id);

            if (editTraining == null)
            {
                return new ApiResponse<AerobicTrainingDto>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "Training Not Found",
                };
            }

            // _context.Entry(aerobicTraining).State = EntityState.Modified;

            try
            {
                editTraining.ActivityDurationMinutes = aerobicTraining.ActivityDurationMinutes;
                editTraining.CalorieBurnt = aerobicTraining.CalorieBurnt;
                editTraining.ActivityType = aerobicTraining.ActivityType;
                editTraining.ActivityDate = aerobicTraining.ActivityDate;
                editTraining.ModificationTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerobicTrainingExists(id))
                {
                    return new ApiResponse<AerobicTrainingDto>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        ErrorMessage = "Training Not Found",
                    };
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<AerobicTrainingDto>
            {
                IsSuccess = true,
                StatusCode = 204,
                Data = new AerobicTrainingDto 
                { 
                    ActivityDate = aerobicTraining.ActivityDate,
                    ActivityDurationMinutes = aerobicTraining.ActivityDurationMinutes,
                    CalorieBurnt = aerobicTraining.CalorieBurnt,
                    ActivityType = aerobicTraining.ActivityType,

                }
            };
        }

        // POST: api/AerobicTrainings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<AerobicTrainingDto>> PostAerobicTraining(AerobicTrainingDto aerobicTraining)
        {
          if (_context.AerobicTrainings == null)
          {
                return new ApiResponse<AerobicTrainingDto>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "Aerobic Trainings Not Defined",
                };
            }


            var username = _http.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = _context.Users.First(u => u.UserName == username).UserId;

            if (userId == Guid.Empty) 
            {
                return new ApiResponse<AerobicTrainingDto>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "User Not Found",
                };
            }

            var newAerobicTraining = new AerobicTraining(userId, aerobicTraining.ActivityType!, aerobicTraining.ActivityDurationMinutes, aerobicTraining.CalorieBurnt, aerobicTraining.ActivityDate);

            _context.AerobicTrainings.Add(newAerobicTraining);
            await _context.SaveChangesAsync();


            return new ApiResponse<AerobicTrainingDto>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = new AerobicTrainingDto
                {
                    ActivityDurationMinutes = aerobicTraining.ActivityDurationMinutes,
                    ActivityType = aerobicTraining.ActivityType,
                    CalorieBurnt = aerobicTraining.CalorieBurnt,
                    ActivityDate = aerobicTraining.ActivityDate
                }
            };
        }

        // DELETE: api/AerobicTrainings/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteAerobicTraining(Guid id)
        {
            if (_context.AerobicTrainings == null)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "Aerobic Trainings Not Defined",
                };
            }
            var aerobicTraining = await _context.AerobicTrainings.FindAsync(id);
            if (aerobicTraining == null)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "Aerobic Training Not Found",
                };
            }

            _context.AerobicTrainings.Remove(aerobicTraining);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                IsSuccess = false,
                StatusCode = 204,
                Data = "No Content"
            };
        }
        [HttpGet("export")]
        public async Task<IActionResult> exportTrainings() // string dateFrom, string dateTo
        {
            /*if (!DateTime.TryParse(dateFrom, out DateTime startDate) || !DateTime.TryParse(dateTo, out DateTime endDate))
            {
                var errorResponse = new ApiResponse<string>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrorMessage = "Date conversion error",
                    StatusCode = 400
                };

                return BadRequest(errorResponse);
            }*/

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
                WriteIndented = true // Enable pretty formatting
            };

            var exportData = JsonSerializer.Serialize(trainingsData, options);
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(exportData);

            Response.Headers.Add("Content-Disposition", "attachment; filename=Aerobic Trainings.json");
            return File(fileBytes, ExportContentType, DefaultExportFileName);
        }

        private bool AerobicTrainingExists(Guid id)
        {
            return (_context.AerobicTrainings?.Any(e => e.AerobicTrainingId == id)).GetValueOrDefault();
        }
    }
}
