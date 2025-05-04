using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessTracker.WebAPI.Services
{
    public class AerobicTrainingsService : IAerobicTrainingsService
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;

        public AerobicTrainingsService(TrainingsContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task<ApiResponse<AerobicTrainingDto>> DeleteAerobicTraining(Guid id)
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

            _context.AerobicTrainings.Remove(aerobicTraining);
            await _context.SaveChangesAsync();

            return new ApiResponse<AerobicTrainingDto>
            {
                IsSuccess = false,
                StatusCode = 204,
            };
        }

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

        private bool AerobicTrainingExists(Guid id)
        {
            return (_context.AerobicTrainings?.Any(e => e.AerobicTrainingId == id)).GetValueOrDefault();
        }
    }
}
