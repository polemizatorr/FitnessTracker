using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Interfaces
{
    public interface IStrengthTrainingsService
    {
        Task<ApiResponse<IEnumerable<StrengthTrainingResponse>>> GetStrengthTrainings();

        Task<ApiResponse<IEnumerable<StrengthTrainingResponse>>> GetStrengthTraining(string username);

        Task<ApiResponse<StrengthTraining>> GetStrengthTraining(Guid id);

        Task<ApiResponse<StrengthTraining>> PutStrenghtTraining(Guid id, StrengthTrainingDTO strenghtTrainingData);

        Task<ApiResponse<StrengthTraining>> PostStrenghtTraining(StrengthTrainingDTO strenghtTrainingData);

        Task<ApiResponse<StrengthTraining>> DeleteStrenghtTraining(Guid id);
    }
}
