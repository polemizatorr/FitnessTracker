using FitnessTracker.WebAPI.Utility;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UnitTests
{
    public class AuthTests
    {

        private readonly IConfiguration _configuration;

        public AuthTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "MyFakeTestSecret" },
                    { "JWT:Issuer", "TestIssuer"},
                    { "JWT:Audience", "TestAudience" }
                })
                .Build();
        }
        [Fact]
        public void GenerateJwtTokenWithClaims()
        {
            // Arrange
            var username = "test1";

            // Act
            var token = UserUtility.GenerateToken(username, _configuration);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var usernameFromTokenClaims = jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;


            // Assert
            Assert.NotNull(token);
            Assert.NotNull(usernameFromTokenClaims);
            Assert.True(token.Length > 0);
            Assert.Equal(username, usernameFromTokenClaims);
        }
    }
}