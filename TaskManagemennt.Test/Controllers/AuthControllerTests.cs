using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.Utility.JWTTokenGenrator;
using Xunit;



namespace TaskManagemennt.Test.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IEmployeeServices> _employeeServiceMock;
        private readonly GenrateJWTTokenGenrater _tokenGen;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeServices>();

            // create a simple in-memory configuration for token generation (used only to produce a token string)
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "TestKey0123456789_TestKey0123456789"},
                {"Jwt:Issuer", "testIssuer"},
                {"Jwt:Audience", "testAudience"}
            };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            _tokenGen = new GenrateJWTTokenGenrater(configuration);

            _controller = new AuthController(_employeeServiceMock.Object, _tokenGen);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithToken_WhenCredentialsValid()
        {
            var password = "pass123";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var emp = new EmployeeModel { Id = 1, Email = "u@x.com", password = hash, role = RoleModel.Employee };

            _employeeServiceMock.Setup(s => s.GetByEmilEmployee("u@x.com")).ReturnsAsync(emp);

            var result = await _controller.Login(new LoginModel { email = "u@x.com", password = password });

            var ok = Assert.IsType<OkObjectResult>(result);
            var tokenObj = ok.Value;
            var tokenProp = tokenObj.GetType().GetProperty("Token");
            Assert.NotNull(tokenProp);
            var tokenValue = tokenProp.GetValue(tokenObj) as string;
            Assert.False(string.IsNullOrEmpty(tokenValue));
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordInvalid()
        {
            var correctHash = BCrypt.Net.BCrypt.HashPassword("correct");
            var emp = new EmployeeModel { Id = 2, Email = "e@x.com", password = correctHash, role = RoleModel.Employee };
            _employeeServiceMock.Setup(s => s.GetByEmilEmployee("e@x.com")).ReturnsAsync(emp);

            var result = await _controller.Login(new LoginModel { email = "e@x.com", password = "wrong" });

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid password", unauthorized.Value);
        }

        [Fact]
        public async Task Login_ReturnsNotFound_WhenUserNotFound()
        {
            _employeeServiceMock.Setup(s => s.GetByEmilEmployee("not@x.com")).ReturnsAsync((EmployeeModel)null);

            var result = await _controller.Login(new LoginModel { email = "not@x.com", password = "x" });

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFound.Value);
        }

        [Fact]
        public async Task Login_ReturnsNotFound_WhenModelNull()
        {
            var result = await _controller.Login(null);
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFound.Value);
        }
    }
}