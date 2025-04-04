using FitnessTracker.WebAPI.ApiResponse;
using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.WebAPI.Controllers.Security
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly TrainingsContext _context;
        private readonly IConfiguration _configuration;
        public SecurityController(TrainingsContext context, IConfiguration configuration) 
        { 
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public ApiResponse<string> Register(RegisterUserDTO user)
        {
            if (!isValidRegisterUserData(user))
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    ErrorMessage = "Invalid Username"
                };

            var newUser = new User(user.UserName, user.Email, user.Password, user.FirstName, user.LastName);


            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return new ApiResponse<string>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ApiResponse<string> Login(UserDto user)
        {
            if (!ValidateUser(user)) 
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    ErrorMessage = "Invalid Credentials"
                };
            }

            var token = UserUtility.GenerateToken(user.Username, _configuration);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                Data = token,
                StatusCode = 200
            };

        }

        private bool ValidateUser(UserDto user)
        {
            if (user == null) 
            {
                throw new ArgumentNullException("user is null");
            }

            var loginUser = _context.Users.FirstOrDefault(u => u.UserName == user.Username);
            if (loginUser == null)
            {
                return false;
            }

            if (!loginUser.Password.Equals(user.Password)) return false;

            return true;
        }

        private bool isValidRegisterUserData(RegisterUserDTO user)
        {
            if (!user.isvalidUserData()) return false;

            var newUser = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);

            if (newUser != null) return false;

            return true;
        }
    }
}
