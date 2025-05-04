using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;

namespace FitnessTracker.WebAPI.Interfaces
{
    public interface ISecurityService
    {
        Task<ApiResponse<string>> Register(RegisterUserDTO user);

        ApiResponse<string> Login(UserDto user);
    }
}
