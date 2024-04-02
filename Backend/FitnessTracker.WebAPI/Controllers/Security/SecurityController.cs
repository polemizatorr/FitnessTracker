using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
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
        [Route("token")]
        [AllowAnonymous]
        public IResult GenerateToken([FromBody]UserDto user)
        {

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Username", user.Username),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),

                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);
                return Results.Ok(stringToken);
           
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
    }
}
