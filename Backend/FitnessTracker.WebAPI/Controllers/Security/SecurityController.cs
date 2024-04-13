using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public SecurityController(TrainingsContext context) 
        { 
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterUserDTO user)
        {
            if (!isValidRegisterUserData(user)) return BadRequest(ModelState);

            var newUser = new User(user.UserName, user.Email, user.Password, user.FirstName, user.LastName);


            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IResult Login(UserDto user)
        {
            if (!ValidateUser(user)) 
            {
                return Results.Problem("Invalid username or password");
            }

            var token = UserUtility.GenerateToken(user.Username);

            return Results.Ok(token);

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
