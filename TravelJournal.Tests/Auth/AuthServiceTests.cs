using Moq;
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.Services;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Domain.Models;
using TravelJournal.Domain.ValueObjects;

namespace TravelJournal.Tests.Auth;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IHasherPassword> _mockPasswordHasher;
    private readonly Mock<ITokenGenerator> _mockTokenGenerator;
    private readonly AuthService _service;
    
    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IHasherPassword>();
        _mockTokenGenerator = new Mock<ITokenGenerator>();
        
        _service = new AuthService(
            _mockUserRepository.Object,
            _mockPasswordHasher.Object,
            _mockTokenGenerator.Object
        );
    }
    
    [Fact]
    public async Task RegisterAsync_NewUser_ReturnsAuthResponse()
    {
        var request = new RegisterRequest("newuser", "newuser@example.com", "password123");
        const string hashedPassword = "hashed_password_123";
        const int userId = 1;
        const string token = "test_token";

        _mockUserRepository.Setup(r => r.ExistsByName(request.Name)).ReturnsAsync(false);
        _mockUserRepository.Setup(r => r.ExistsByEmail(It.IsAny<Email>())).ReturnsAsync(false);
        _mockPasswordHasher.Setup(h => h.Hash(request.Password)).Returns(hashedPassword);
        _mockTokenGenerator.Setup(g => g.GenerateToken(It.IsAny<int>(), request.Name)).Returns(token);
        
        _mockUserRepository
            .Setup(r => r.AddUser(It.IsAny<User>()))
            .Callback<User>(user => { user.Id = userId; })
            .Returns(Task.CompletedTask);
        
        var result = await _service.RegisterAsync(request);
        
        Assert.Equal(token, result.Token);
        Assert.Equal(request.Name, result.User.Name);
        Assert.Equal(request.Email, result.User.Email);
        
        _mockUserRepository.Verify(r => r.ExistsByName(request.Name), Times.Once);
        _mockUserRepository.Verify(r => r.ExistsByEmail(It.IsAny<Email>()), Times.Once);
        _mockUserRepository.Verify(r => r.AddUser(It.IsAny<User>()), Times.Once);
    }
    
    [Fact]
    public async Task RegisterAsync_UserNameExists_ThrowsArgumentException()
    {
        var request = new RegisterRequest("existinguser", "newuser@example.com", "password123");
        
        _mockUserRepository.Setup(r => r.ExistsByName(request.Name)).ReturnsAsync(true);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RegisterAsync(request)
        );
        
        Assert.Equal("User with this name already exists", exception.Message);
        _mockUserRepository.Verify(r => r.ExistsByName(request.Name), Times.Once);
        _mockUserRepository.Verify(r => r.AddUser(It.IsAny<User>()), Times.Never);
    }
    
    [Fact]
    public async Task RegisterAsync_EmailExists_ThrowsArgumentException()
    {
        var request = new RegisterRequest("newuser", "existing@example.com", "password123");
        
        _mockUserRepository.Setup(r => r.ExistsByName(request.Name)).ReturnsAsync(false);
        _mockUserRepository.Setup(r => r.ExistsByEmail(It.IsAny<Email>())).ReturnsAsync(true);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RegisterAsync(request)
        );
        
        Assert.Equal("User with this email already exists", exception.Message);
        _mockUserRepository.Verify(r => r.ExistsByEmail(It.IsAny<Email>()), Times.Once);
        _mockUserRepository.Verify(r => r.AddUser(It.IsAny<User>()), Times.Never);
    }
    
    [Fact]
    public async Task LoginAsync_ValidEmail_ReturnsAuthResponse()
    {
        var request = new LoginRequest("user@example.com", "password123");
        const int userId = 1;
        const string userName = "testuser";
        const string token = "test_token";
        var creationTime = DateTime.UtcNow;
        
        var email = Email.Create(request.Login);
        var user = new User(userName, email, "hashed_password") { Id = userId, CreatedAt = creationTime };
        
        _mockUserRepository.Setup(r => r.FindUserByEmail(It.IsAny<Email>())).ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(request.Password, user.Password)).Returns(true);
        _mockTokenGenerator.Setup(g => g.GenerateToken(userId, userName)).Returns(token);
        
        var result = await _service.LoginAsync(request);
        
        Assert.Equal(token, result.Token);
        Assert.Equal(userId, result.User.Id);
        Assert.Equal(userName, result.User.Name);
        Assert.Equal(email.Value, result.User.Email);
        
        _mockUserRepository.Verify(r => r.FindUserByEmail(It.IsAny<Email>()), Times.Once);
        _mockPasswordHasher.Verify(h => h.Verify(request.Password, user.Password), Times.Once);
    }
    
    [Fact]
    public async Task LoginAsync_ValidUsername_ReturnsAuthResponse()
    {
        var request = new LoginRequest("testuser", "password123");
        const int userId = 1;
        var email = Email.Create("test@example.com");
        const string token = "test_token";
        var creationTime = DateTime.UtcNow;
        
        var user = new User(request.Login, email, "hashed_password") { Id = userId, CreatedAt = creationTime };
        
        _mockUserRepository.Setup(r => r.FindUserByEmail(It.IsAny<Email>())).ThrowsAsync(new ArgumentException());
        _mockUserRepository.Setup(r => r.FindUserByName(request.Login)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(request.Password, user.Password)).Returns(true);
        _mockTokenGenerator.Setup(g => g.GenerateToken(userId, request.Login)).Returns(token);
        
        var result = await _service.LoginAsync(request);
        
        Assert.Equal(token, result.Token);
        Assert.Equal(userId, result.User.Id);
        Assert.Equal(request.Login, result.User.Name);
        
        _mockUserRepository.Verify(r => r.FindUserByName(request.Login), Times.Once);
    }
    
    [Fact]
    public async Task LoginAsync_UserNotFound_ThrowsUnauthorizedAccessException()
    {
        var request = new LoginRequest("nonexistent", "password123");
        
        _mockUserRepository.Setup(r => r.FindUserByEmail(It.IsAny<Email>())).ThrowsAsync(new ArgumentException());
        _mockUserRepository.Setup(r => r.FindUserByName(request.Login)).ReturnsAsync((User?)null);
        
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.LoginAsync(request)
        );
        
        _mockUserRepository.Verify(r => r.FindUserByName(request.Login), Times.Once);
    }
    
    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        var request = new LoginRequest("testuser", "wrongpassword");
        const int userId = 1;
        var email = Email.Create("test@example.com");
        
        var user = new User(request.Login, email, "hashed_password") { Id = userId };
        
        _mockUserRepository.Setup(r => r.FindUserByEmail(It.IsAny<Email>())).ThrowsAsync(new ArgumentException());
        _mockUserRepository.Setup(r => r.FindUserByName(request.Login)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(request.Password, user.Password)).Returns(false);
        
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.LoginAsync(request)
        );
        
        _mockPasswordHasher.Verify(h => h.Verify(request.Password, user.Password), Times.Once);
        _mockTokenGenerator.Verify(g => g.GenerateToken(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }
} 