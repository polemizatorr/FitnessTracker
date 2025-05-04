using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Interfaces
{
    public interface ISetsService
    {
        ApiResponse<IEnumerable<Set>> GetSetsForTraining(Guid trainingId);

        Task<ApiResponse<Set>> CreateSet(Guid strengthTrainingId, SetDto set);

        Task<ApiResponse<Set>> EditSet(Guid setID, SetDto set);

        Task<ApiResponse<Set>> DeleteSet(Guid id);

    }
}
