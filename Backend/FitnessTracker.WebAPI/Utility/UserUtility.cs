using FitnessTracker.WebAPI.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.WebAPI.Utility
{
    public class UserUtility
    {
        private const string _tokenSecret = "QLLJuCyxYFLqgkzDrBqq0o1ObZD1ZamK";
        public static string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_tokenSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
            };

            var token = new JwtSecurityToken("FitnessTrackerISS",
               "https://fitnestracker.com",
                claims,
                expires: DateTime.Now.AddMinutes(360),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
        public static bool ValidateUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            return true;
        }
    }
}
