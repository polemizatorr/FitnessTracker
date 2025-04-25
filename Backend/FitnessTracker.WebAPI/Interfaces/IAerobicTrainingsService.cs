using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Interfaces
{
    public interface IAerobicTrainingsService
    {
        Task<ApiResponse<IEnumerable<AerobicTrainingDto>>> GetAerobicTrainings();

        Task<ApiResponse<IEnumerable<AerobicTrainingDto>>> GetAerobicTrainingsForUser(string username);

        Task<ApiResponse<AerobicTrainingDto>> GetAerobicTraining(Guid id);

        Task<ApiResponse<AerobicTrainingDto>> PutAerobicTraining(Guid id, AerobicTrainingDto aerobicTraining);

        Task<ApiResponse<AerobicTrainingDto>> PostAerobicTraining(AerobicTrainingDto aerobicTraining);

        Task<ApiResponse<AerobicTrainingDto>> DeleteAerobicTraining(Guid id);
    }
}
