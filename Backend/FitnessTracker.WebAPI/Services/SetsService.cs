using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Services
{
    public class SetsService : ISetsService
    {
        private readonly TrainingsContext _context;
        private readonly IConfiguration _configuration;

        public SetsService(TrainingsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Task<ActionResult<Set>> CreateSet(Guid strengthTrainingId, SetDto set)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteSet(Guid id)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<IEnumerable<Set>> GetSetsForTrining(Guid trainingId)
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
