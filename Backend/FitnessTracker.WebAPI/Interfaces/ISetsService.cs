using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Interfaces
{
    public interface ISetsService
    {
        ApiResponse<IEnumerable<Set>> GetSetsForTrining(Guid trainingId);

        Task<ActionResult<Set>> CreateSet(Guid strengthTrainingId, SetDto set);

        Task<IActionResult> DeleteSet(Guid id);

    }
}
