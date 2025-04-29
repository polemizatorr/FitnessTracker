using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.WebAPI.ApiResponse;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace FitnessTracker.WebAPI.Services
{
    public class UsersService : IUsersService
    {
        private readonly TrainingsContext _context;

        public UsersService(TrainingsContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<User>> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new ApiResponse<User>
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found.",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new ApiResponse<User>
            {
                IsSuccess = true,
                Data = user, // You could also return null or just omit Data if you prefer
                StatusCode = 204
            };
        }

        public async Task<ApiResponse<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return new ApiResponse<User>
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            return new ApiResponse<User>
            {
                IsSuccess = true,
                Data = user,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public ApiResponse<IEnumerable<User>> GetUsers()
        {
            var users = _context.Users.ToList();

            return new ApiResponse<IEnumerable<User>>
            {
                IsSuccess = true,
                Data = users,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<ApiResponse<User>> PostUser(CreateUserDTO user)
        {
            var newUser = new User(user.UserName, user.Email, user.Password, user.FirstName, user.LastName);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return new ApiResponse<User>
            {
                IsSuccess = true,
                Data = newUser,
                StatusCode = StatusCodes.Status201Created
            };
        }

        public async Task<ApiResponse<User>> PutUser(Guid id, User user)
        {
            if (id != user.UserId)
            {
                return new ApiResponse<User>
                {
                    IsSuccess = false,
                    ErrorMessage = "User ID mismatch.",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return new ApiResponse<User>
                    {
                        IsSuccess = false,
                        ErrorMessage = "User not found.",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<User>
            {
                IsSuccess = true,
                Data = user,
                StatusCode = 204
            };
        }

        public async Task<ApiResponse<User>> DeleteUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return new ApiResponse<User>
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found.",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new ApiResponse<User>
            {
                IsSuccess = true,
                Data = user,
                StatusCode = 204
            };
        }
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
