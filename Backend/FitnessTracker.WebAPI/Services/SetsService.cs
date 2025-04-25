using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessTracker.WebAPI.Services
{
    public class SetsService : ISetsService
    {
        private readonly TrainingsContext _context;
        private readonly IHttpContextAccessor _http;

        public SetsService(TrainingsContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task<ApiResponse<Set>> CreateSet(Guid strengthTrainingId, SetDto set)
        {
            if (_context.Sets == null)
            {
                return new ApiResponse<Set>
                {
                    IsSuccess = false,
                    ErrorMessage = "Entity set 'TrainingsContext.Sets' is null.",
                    StatusCode = 500
                };
            }

            var username = _http.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return new ApiResponse<Set>
                {
                    IsSuccess = false,
                    ErrorMessage = "No user found for the given username.",
                    StatusCode = 404
                };
            }

            var newSet = new Set(strengthTrainingId, set.RepetitionsNumber, set.ExerciseName!, set.ExhaustionLevel, set.Weight);

            _context.Sets.Add(newSet);
            await _context.SaveChangesAsync();

            return new ApiResponse<Set>
            {
                IsSuccess = true,
                Data = newSet,
                StatusCode = 201
            };
        }

        public async Task<ApiResponse<Set>> DeleteSet(Guid id)
        {
            if (_context.Sets == null)
            {
                return new ApiResponse<Set>
                {
                    IsSuccess = false,
                    ErrorMessage = "Set collection not found.",
                    StatusCode = 404
                };
            }

            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return new ApiResponse<Set>
                {
                    IsSuccess = false,
                    ErrorMessage = "Set not found.",
                    StatusCode = 404
                };
            }

            _context.Sets.Remove(set);
            await _context.SaveChangesAsync();

            return new ApiResponse<Set>
            {
                IsSuccess = true,
                Data = set,
                StatusCode = 200
            };
        }

        public ApiResponse<IEnumerable<Set>> GetSetsForTraining(Guid trainingId)
        {
            if (_context.Sets == null)
            {
                return new ApiResponse<IEnumerable<Set>>
                {
                    IsSuccess = false,
                    ErrorMessage = "Sets collection is null.",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var sets =  _context.Sets.Where(s => s.StrenghtTrainingId == trainingId).ToList();

            if (sets == null || !sets.Any())
            {
                return new ApiResponse<IEnumerable<Set>>
                {
                    IsSuccess = false,
                    ErrorMessage = "No sets found for the given training ID.",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            return new ApiResponse<IEnumerable<Set>>
            {
                IsSuccess = true,
                Data = sets,
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
