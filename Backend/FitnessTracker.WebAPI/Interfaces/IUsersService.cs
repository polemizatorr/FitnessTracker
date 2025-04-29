using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebAPI.Interfaces
{
    public interface IUsersService
    {
        ApiResponse<IEnumerable<User>> GetUsers();

        Task<ApiResponse<User>> GetUser(Guid id);

        Task<ApiResponse<User>> PostUser(CreateUserDTO user);

        Task<ApiResponse<User>> PutUser(Guid id, User user);

        Task<ApiResponse<User>> DeleteUser(Guid id);

        Task<ApiResponse<User>> DeleteUserByUsername(string username);
    }
}
