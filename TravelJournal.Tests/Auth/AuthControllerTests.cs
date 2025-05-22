using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using TravelJournal.Api.Controllers;
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;

namespace TravelJournal.Tests.Auth;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;
    
    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }
    
    [Fact]
    public async Task Register_Success()
    {
        var request = new RegisterRequest("testuser", "test@example.com", "password123");
        var expectedResponse = new AuthResponse(
            "test_token",
            new UserResponse(1, "testuser", "test@example.com", DateTime.UtcNow)
        );
        
        _mockAuthService
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ReturnsAsync(expectedResponse);
        
        var result = await _controller.Register(request);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<AuthResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Token, returnValue.Token);
        Assert.Equal(expectedResponse.User.Name, returnValue.User.Name);
        Assert.Equal(expectedResponse.User.Email, returnValue.User.Email);
    }
    
    [Fact]
    public async Task Register_Duplicate()
    {
        var request = new RegisterRequest("existinguser", "existing@example.com", "password123");
        const string errorMessage = "User with this name already exists";
        
        _mockAuthService
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ThrowsAsync(new ArgumentException(errorMessage));
        
        var result = await _controller.Register(request);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(
            JsonSerializer.Serialize(badRequestResult.Value)
        );
        
        Assert.True(jsonElement.TryGetProperty("message", out JsonElement messageElement));
        Assert.Equal(errorMessage, messageElement.GetString());
    }
    
    [Fact]
    public async Task Login_Success()
    {
        var request = new LoginRequest("testuser", "password123");
        var expectedResponse = new AuthResponse(
            "test_token",
            new UserResponse(1, "testuser", "test@example.com", DateTime.UtcNow)
        );
        
        _mockAuthService
            .Setup(s => s.LoginAsync(It.IsAny<LoginRequest>()))
            .ReturnsAsync(expectedResponse);
        
        var result = await _controller.Login(request);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<AuthResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Token, returnValue.Token);
        Assert.Equal(expectedResponse.User.Name, returnValue.User.Name);
    }
    
    [Fact]
    public async Task Login_Failed()
    {
        var request = new LoginRequest("wronguser", "wrongpassword");
        const string errorMessage = "Invalid credentials";
        
        _mockAuthService
            .Setup(s => s.LoginAsync(It.IsAny<LoginRequest>()))
            .ThrowsAsync(new UnauthorizedAccessException(errorMessage));
        
        var result = await _controller.Login(request);
        
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(
            JsonSerializer.Serialize(unauthorizedResult.Value)
        );
        
        Assert.True(jsonElement.TryGetProperty("message", out JsonElement messageElement));
        Assert.Equal(errorMessage, messageElement.GetString());
    }
} 