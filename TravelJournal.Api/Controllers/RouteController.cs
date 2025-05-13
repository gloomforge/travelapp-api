using Microsoft.AspNetCore.Mvc;
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;

namespace TravelJournal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouteController(IRouteService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RouteResponse>> Create([FromBody] CreateRouteRequest request) => 
        await service.CreateRouteAsync(request);
    
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RouteResponse>> Update(int id, [FromBody] UpdateRouteRequest request)
    {
        try
        {
            var result = await service.UpdateRouteAsync(id, request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await service.DeleteRouteAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RouteResponse>> GetById(int id)
    {
        try
        {
            var result = await service.FindRouteByIdAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("location/{location}")]
    public async Task<ActionResult<List<RouteResponse>>> GetByLocation(string location)
    {
        var result = await service.FindRoutesByLocationAsync(location);
        return Ok(result);
    }

    [HttpGet("country/{country}")]
    public async Task<ActionResult<List<RouteResponse>>> GetByCountry(string country)
    {
        var result = await service.FindRoutesByCountryAsync(country);
        return Ok(result);
    }

    [HttpGet("city/{city}")]
    public async Task<ActionResult<List<RouteResponse>>> GetByCity(string city)
    {
        var result = await service.FindRoutesByCityAsync(city);
        return Ok(result);
    }
}