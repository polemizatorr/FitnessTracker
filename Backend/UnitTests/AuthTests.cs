using FitnessTracker.WebAPI.DatabaseContext;
using FitnessTracker.WebAPI.Entities.DTO;
using FitnessTracker.WebAPI.Entities.Models;
using FitnessTracker.WebAPI.Services;
using FitnessTracker.WebAPI.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UnitTests
{
    public class AuthTests
    {
        private readonly IConfiguration _configuration;
        private readonly DbContextOptions<TrainingsContext> _options;

        public AuthTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "your-very-long-secret-key-here-1234567890abcd" },
                    { "JWT:Issuer", "TestIssuer"},
                    { "JWT:Audience", "TestAudience" }
                })
                .Build();

            _options = new DbContextOptionsBuilder<TrainingsContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        }
        [Fact]
        public void GenerateJwtTokenWithClaims_ShouldGenerateValidToken()
        {
            // Arrange
            var username = "test-username-01";

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

        [Fact]
        public async void RegisterUser_ShouldCreateNewUser() 
        {
            // Arrange
            var successResponseCode = 200;
            var expectedUsersCount = 1;

            using (var context = new TrainingsContext(_options))
            {
                var service = new SecurityService(context, _configuration);

                var newUser = new RegisterUserDTO("john_doe", "john.doe@example.com", "password123", "John", "Doe");

                // Act
                var result = await service.Register(newUser);

                // Assert
                Assert.Equal(result.StatusCode, successResponseCode);
                Assert.True(result.IsSuccess);
                Assert.True(context.Users.Any());
                Assert.Equal(context.Users.Count(), expectedUsersCount);
            }
        }

        [Fact]
        public async void RegisterUser_ShouldNotCreateUserWithExistingUsername()
        {
            // Arrange
            var badRequestResponseCode = 400;
            var expectedUsersCount = 1;
            var expectedErrorMessage = "Invalid Username";

            using (var context = new TrainingsContext(_options))
            {
                var service = new SecurityService(context, _configuration);

                var existingUser = new User("john_doe", "john.doe@example.com", "password123", "John", "Doe");

                context.Users.Add(existingUser);
                context.SaveChanges();

                var newUser = new RegisterUserDTO("john_doe", "john.doe@example.com", "password123", "John", "Doe");

                // Act
                var result = await service.Register(newUser);

                // Assert
                Assert.Equal(result.StatusCode, badRequestResponseCode);
                Assert.False(result.IsSuccess);
                Assert.Equal(result.ErrorMessage, expectedErrorMessage);
                Assert.Equal(context.Users.Count(), expectedUsersCount);
            }
        }

        [Fact]
        public async void RegisterUser_ShouldNotCreateUserWithoutAllFields()
        {
            // Arrange
            var badRequestResponseCode = 400;
            var expectedUsersCount = 0;
            var expectedErrorMessage = "Invalid Username";

            using (var context = new TrainingsContext(_options))
            {
                var service = new SecurityService(context, _configuration);

                var newUser = new RegisterUserDTO("john_doe", "john.doe@example.com", "password123", "", "");

                // Act
                var result = await service.Register(newUser);

                // Assert
                Assert.Equal(result.StatusCode, badRequestResponseCode);
                Assert.False(result.IsSuccess);
                Assert.Equal(result.ErrorMessage, expectedErrorMessage);
                Assert.Equal(context.Users.Count(), expectedUsersCount);
            }
        }

        [Fact]
        public void LoginUser_ShouldLoginAsExistingUser()
        {
            // Arrange
            var successRequestResponseCode = 200;
            var expectedUsersCount = 1;

            using (var context = new TrainingsContext(_options))
            {
                var service = new SecurityService(context, _configuration);

                var existingUser = new User("john_doe", "john.doe@example.com", "password123", "John", "Doe");

                context.Users.Add(existingUser);
                context.SaveChanges();

                var loginUser = new UserDto(existingUser.UserName, existingUser.Password);

                // Act
                var result = service.Login(loginUser);

                // Assert
                Assert.Equal(result.StatusCode, successRequestResponseCode);
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.True(result.Data.Length > 0);
                Assert.Equal(context.Users.Count(), expectedUsersCount);
            }
        }

        [Fact]
        public void LoginNonexistingUser_ShouldReturnFailureResponse()
        {
            // Arrange
            var unauthorizedRequestResponseCode = 401;
            var expectedUsersCount = 0;

            using (var context = new TrainingsContext(_options))
            {
                var service = new SecurityService(context, _configuration);

                var testUsername = "test-01";
                var testPassword = "password-01";
                
                var loginUser = new UserDto(testUsername, testPassword);

                // Act
                var result = service.Login(loginUser);

                // Assert
                Assert.Equal(result.StatusCode, unauthorizedRequestResponseCode);
                Assert.False(result.IsSuccess);
                Assert.Null(result.Data);
                Assert.Equal(context.Users.Count(), expectedUsersCount);
            }
        }
    }
}