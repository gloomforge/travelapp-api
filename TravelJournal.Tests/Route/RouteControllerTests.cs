using Microsoft.AspNetCore.Mvc;
using Moq;
using TravelJournal.Api.Controllers;
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;

namespace TravelJournal.Tests.Route;

public class RouteControllerTests
{
    private readonly Mock<IRouteService> _mockRouteService;
    private readonly RouteController _controller;

    public RouteControllerTests()
    {
        _mockRouteService = new Mock<IRouteService>();
        _controller = new RouteController(_mockRouteService.Object);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturnCreatedResult()
    {
        var request = new CreateRouteRequest("Test Location", "Test Country", "Test City", 1);
        var response = new RouteResponse(1, "Test Location", "Test Country", "Test City");
        
        _mockRouteService.Setup(s => s.CreateRouteAsync(request)).ReturnsAsync(response);

        var result = await _controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<RouteResponse>(createdResult.Value);
        
        Assert.Equal(response.Id, returnValue.Id);
        Assert.Equal(response.LocationName, returnValue.LocationName);
        Assert.Equal(response.Country, returnValue.Country);
        Assert.Equal(response.City, returnValue.City);
        
        Assert.Equal("GetById", createdResult.ActionName);
        Assert.Equal(response.Id, createdResult.RouteValues?["id"]);
        
        _mockRouteService.Verify(s => s.CreateRouteAsync(request), Times.Once);
    }

    [Fact]
    public async Task Create_WithInvalidTripId_ShouldReturnNotFoundWithErrorMessage()
    {
        var request = new CreateRouteRequest("Test Location", "Test Country", "Test City", 999);
        const string errorMessage = "Trip with id 999 not found";
        
        _mockRouteService.Setup(s => s.CreateRouteAsync(request))
            .ThrowsAsync(new KeyNotFoundException(errorMessage));

        var result = await _controller.Create(request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var errorObj = Assert.IsAssignableFrom<object>(notFoundResult.Value);
        
        var errorObjDict = errorObj.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(errorObj)?.ToString());
        
        Assert.Equal(errorMessage, errorObjDict["message"]);
        Assert.Contains("Please create a trip first", errorObjDict["error"]);
        
        _mockRouteService.Verify(s => s.CreateRouteAsync(request), Times.Once);
    }

    [Fact]
    public async Task GetById_WithExistingRoute_ShouldReturnOkResultWithRoute()
    {
        const int id = 1;
        var response = new RouteResponse(id, "Test Location", "Test Country", "Test City");
        
        _mockRouteService.Setup(s => s.FindRouteByIdAsync(id)).ReturnsAsync(response);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<RouteResponse>(okResult.Value);
        
        Assert.Equal(response.Id, returnValue.Id);
        Assert.Equal(response.LocationName, returnValue.LocationName);
        Assert.Equal(response.Country, returnValue.Country);
        Assert.Equal(response.City, returnValue.City);
        
        _mockRouteService.Verify(s => s.FindRouteByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetById_WithNonExistingRoute_ShouldReturnNotFound()
    {
        const int id = 999;
        
        _mockRouteService.Setup(s => s.FindRouteByIdAsync(id))
            .ThrowsAsync(new KeyNotFoundException($"Route with id {id} was not found"));

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundResult>(result.Result);
        
        _mockRouteService.Verify(s => s.FindRouteByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task ByLocation_ShouldReturnOkResultWithRoutes()
    {
        const string location = "Test Location";
        var responses = new List<RouteResponse>
        {
            new(1, location, "Country 1", "City 1"),
            new(2, location, "Country 2", "City 2")
        };
        
        _mockRouteService.Setup(s => s.FindRoutesByLocationAsync(location)).ReturnsAsync(responses);

        var result = await _controller.ByLocation(location);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<RouteResponse>>(okResult.Value);
        
        Assert.Equal(responses.Count, returnValue.Count);
        Assert.Equal(responses[0].Id, returnValue[0].Id);
        Assert.Equal(responses[1].Id, returnValue[1].Id);
        
        _mockRouteService.Verify(s => s.FindRoutesByLocationAsync(location), Times.Once);
    }

    [Fact]
    public async Task ByCountry_ShouldReturnOkResultWithRoutes()
    {
        const string country = "Test Country";
        var responses = new List<RouteResponse>
        {
            new(1, "Location 1", country, "City 1"),
            new(2, "Location 2", country, "City 2")
        };
        
        _mockRouteService.Setup(s => s.FindRoutesByCountryAsync(country)).ReturnsAsync(responses);

        var result = await _controller.ByCountry(country);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<RouteResponse>>(okResult.Value);
        
        Assert.Equal(responses.Count, returnValue.Count);
        Assert.Equal(responses[0].Id, returnValue[0].Id);
        Assert.Equal(responses[1].Id, returnValue[1].Id);
        
        _mockRouteService.Verify(s => s.FindRoutesByCountryAsync(country), Times.Once);
    }

    [Fact]
    public async Task ByCity_ShouldReturnOkResultWithRoutes()
    {
        const string city = "Test City";
        var responses = new List<RouteResponse>
        {
            new(1, "Location 1", "Country 1", city),
            new(2, "Location 2", "Country 2", city)
        };
        
        _mockRouteService.Setup(s => s.FindRoutesByCityAsync(city)).ReturnsAsync(responses);

        var result = await _controller.ByCity(city);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<RouteResponse>>(okResult.Value);
        
        Assert.Equal(responses.Count, returnValue.Count);
        Assert.Equal(responses[0].Id, returnValue[0].Id);
        Assert.Equal(responses[1].Id, returnValue[1].Id);
        
        _mockRouteService.Verify(s => s.FindRoutesByCityAsync(city), Times.Once);
    }

    [Fact]
    public async Task Update_WithExistingRoute_ShouldReturnOkResultWithUpdatedRoute()
    {
        const int id = 1;
        var request = new UpdateRouteRequest("Updated Location", "Updated Country", "Updated City");
        var response = new RouteResponse(id, "Updated Location", "Updated Country", "Updated City");
        
        _mockRouteService.Setup(s => s.UpdateRouteAsync(id, request)).ReturnsAsync(response);

        var result = await _controller.Update(id, request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<RouteResponse>(okResult.Value);
        
        Assert.Equal(response.Id, returnValue.Id);
        Assert.Equal(response.LocationName, returnValue.LocationName);
        Assert.Equal(response.Country, returnValue.Country);
        Assert.Equal(response.City, returnValue.City);
        
        _mockRouteService.Verify(s => s.UpdateRouteAsync(id, request), Times.Once);
    }

    [Fact]
    public async Task Update_WithNonExistingRoute_ShouldReturnNotFound()
    {
        const int id = 999;
        var request = new UpdateRouteRequest("Updated Location", "Updated Country", "Updated City");
        
        _mockRouteService.Setup(s => s.UpdateRouteAsync(id, request))
            .ThrowsAsync(new KeyNotFoundException($"Route with id {id} was not found"));

        var result = await _controller.Update(id, request);

        Assert.IsType<NotFoundResult>(result.Result);
        
        _mockRouteService.Verify(s => s.UpdateRouteAsync(id, request), Times.Once);
    }

    [Fact]
    public async Task Delete_WithExistingRoute_ShouldReturnNoContent()
    {
        const int id = 1;
        
        _mockRouteService.Setup(s => s.DeleteRouteAsync(id)).ReturnsAsync(true);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
        
        _mockRouteService.Verify(s => s.DeleteRouteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WithNonExistingRoute_ShouldReturnNotFound()
    {
        const int id = 999;
        
        _mockRouteService.Setup(s => s.DeleteRouteAsync(id)).ReturnsAsync(false);

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
        
        _mockRouteService.Verify(s => s.DeleteRouteAsync(id), Times.Once);
    }
} 