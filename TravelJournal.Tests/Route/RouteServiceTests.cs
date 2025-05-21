using Moq;
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.Services;
using TravelJournal.Domain.Interfaces;

namespace TravelJournal.Tests.Route;

public class RouteServiceTests
{
    private readonly Mock<IRouteRepository> _mockRouteRepository;
    private readonly Mock<ITripRepository> _mockTripRepository;
    private readonly RouteService _routeService;

    public RouteServiceTests()
    {
        _mockRouteRepository = new Mock<IRouteRepository>();
        _mockTripRepository = new Mock<ITripRepository>();
        _routeService = new RouteService(_mockRouteRepository.Object, _mockTripRepository.Object);
    }

    [Fact]
    public async Task CreateRouteAsync_WithValidTripId_ShouldReturnRouteResponse()
    {
        var request = new CreateRouteRequest("Test Location", "Test Country", "Test City", 1);

        _mockTripRepository.Setup(repo => repo.ExistsById(1)).ReturnsAsync(true);
        _mockRouteRepository.Setup(repo => repo.AddRoute(It.IsAny<TravelJournal.Domain.Models.Route>()))
            .Callback<TravelJournal.Domain.Models.Route>(r => 
            {
                r.GetType().GetProperty("Id")?.SetValue(r, 1);
            })
            .Returns(Task.CompletedTask);

        var result = await _routeService.CreateRouteAsync(request);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(request.LocationName, result.LocationName);
        Assert.Equal(request.Country, result.Country);
        Assert.Equal(request.City, result.City);
        
        _mockTripRepository.Verify(repo => repo.ExistsById(1), Times.Once);
        _mockRouteRepository.Verify(repo => repo.AddRoute(It.IsAny<TravelJournal.Domain.Models.Route>()), Times.Once);
    }

    [Fact]
    public async Task CreateRouteAsync_WithInvalidTripId_ShouldThrowKeyNotFoundException()
    {
        var request = new CreateRouteRequest("Test Location", "Test Country", "Test City", 999);
        _mockTripRepository.Setup(repo => repo.ExistsById(999)).ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _routeService.CreateRouteAsync(request));
        Assert.Contains("Trip with id 999 not found", exception.Message);
        
        _mockTripRepository.Verify(repo => repo.ExistsById(999), Times.Once);
        _mockRouteRepository.Verify(repo => repo.AddRoute(It.IsAny<TravelJournal.Domain.Models.Route>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRouteAsync_WithExistingRoute_ShouldReturnUpdatedRouteResponse()
    {
        const int id = 1;
        var request = new UpdateRouteRequest("Updated Location", "Updated Country", "Updated City");
        var route = new TravelJournal.Domain.Models.Route("Original Location", "Original Country", "Original City");
        route.GetType().GetProperty("Id")?.SetValue(route, id);
        
        _mockRouteRepository.Setup(repo => repo.FindById(id)).ReturnsAsync(route);
        _mockRouteRepository.Setup(repo => repo.UpdateRoute(It.IsAny<TravelJournal.Domain.Models.Route>()))
            .Returns(Task.CompletedTask);

        var result = await _routeService.UpdateRouteAsync(id, request);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(request.LocationName, result.LocationName);
        Assert.Equal(request.Country, result.Country);
        Assert.Equal(request.City, result.City);
        
        _mockRouteRepository.Verify(repo => repo.FindById(id), Times.Once);
        _mockRouteRepository.Verify(repo => repo.UpdateRoute(It.IsAny<TravelJournal.Domain.Models.Route>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRouteAsync_WithNonExistingRoute_ShouldThrowKeyNotFoundException()
    {
        const int id = 999;
        var request = new UpdateRouteRequest("Updated Location", "Updated Country", "Updated City");
        
        _mockRouteRepository.Setup(repo => repo.FindById(id)).ReturnsAsync((Domain.Models.Route?)null);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _routeService.UpdateRouteAsync(id, request));
        Assert.Contains($"Route with id {id} was not found", exception.Message);
        
        _mockRouteRepository.Verify(repo => repo.FindById(id), Times.Once);
        _mockRouteRepository.Verify(repo => repo.UpdateRoute(It.IsAny<TravelJournal.Domain.Models.Route>()), Times.Never);
    }

    [Fact]
    public async Task DeleteRouteAsync_WithExistingRoute_ShouldReturnTrue()
    {
        const int id = 1;
        var route = new TravelJournal.Domain.Models.Route("Test Location", "Test Country", "Test City");
        route.GetType().GetProperty("Id")?.SetValue(route, id);
        
        _mockRouteRepository.Setup(repo => repo.FindById(id)).ReturnsAsync(route);
        _mockRouteRepository.Setup(repo => repo.DeleteRoute(It.IsAny<TravelJournal.Domain.Models.Route>()))
            .Returns(Task.CompletedTask);

        var result = await _routeService.DeleteRouteAsync(id);

        Assert.True(result);
        
        _mockRouteRepository.Verify(repo => repo.FindById(id), Times.Once);
        _mockRouteRepository.Verify(repo => repo.DeleteRoute(It.IsAny<TravelJournal.Domain.Models.Route>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRouteAsync_WithNonExistingRoute_ShouldReturnFalse()
    {
        const int id = 999;
        
        _mockRouteRepository.Setup(repo => repo.FindById(id)).ReturnsAsync((Domain.Models.Route?)null);

        var result = await _routeService.DeleteRouteAsync(id);

        Assert.False(result);
        
        _mockRouteRepository.Verify(repo => repo.FindById(id), Times.Once);
        _mockRouteRepository.Verify(repo => repo.DeleteRoute(It.IsAny<TravelJournal.Domain.Models.Route>()), Times.Never);
    }

    [Fact]
    public async Task FindRouteByIdAsync_WithExistingRoute_ShouldReturnRouteResponse()
    {
        const int id = 1;
        var route = new TravelJournal.Domain.Models.Route("Test Location", "Test Country", "Test City");
        route.GetType().GetProperty("Id")?.SetValue(route, id);
        
        _mockRouteRepository.Setup(repo => repo.FindById(id)).ReturnsAsync(route);

        var result = await _routeService.FindRouteByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(route.LocationName, result.LocationName);
        Assert.Equal(route.Country, result.Country);
        Assert.Equal(route.City, result.City);
        
        _mockRouteRepository.Verify(repo => repo.FindById(id), Times.Once);
    }

    [Fact]
    public async Task FindRouteByIdAsync_WithNonExistingRoute_ShouldThrowKeyNotFoundException()
    {
        const int id = 999;
        
        _mockRouteRepository.Setup(repo => repo.FindById(id)).ReturnsAsync((Domain.Models.Route?)null);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _routeService.FindRouteByIdAsync(id));
        Assert.Contains($"Route with id {id} was not found", exception.Message);
        
        _mockRouteRepository.Verify(repo => repo.FindById(id), Times.Once);
    }

    [Fact]
    public async Task FindRoutesByLocationAsync_ShouldReturnRouteResponses()
    {
        const string location = "Test Location";
        var routes = new List<TravelJournal.Domain.Models.Route>
        {
            new("Test Location", "Country 1", "City 1"),
            new("Test Location", "Country 2", "City 2")
        };
        
        routes[0].GetType().GetProperty("Id")?.SetValue(routes[0], 1);
        routes[1].GetType().GetProperty("Id")?.SetValue(routes[1], 2);
        
        _mockRouteRepository.Setup(repo => repo.FindRoutesByLocation(location)).ReturnsAsync(routes);

        var results = await _routeService.FindRoutesByLocationAsync(location);

        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
        Assert.Equal(1, results[0].Id);
        Assert.Equal(2, results[1].Id);
        Assert.Equal(location, results[0].LocationName);
        Assert.Equal(location, results[1].LocationName);
        
        _mockRouteRepository.Verify(repo => repo.FindRoutesByLocation(location), Times.Once);
    }
}