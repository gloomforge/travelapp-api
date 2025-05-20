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
    public async Task<ActionResult<RouteResponse>> Create([FromBody] CreateRouteRequest dto)
        => Ok(await service.CreateRouteAsync(dto));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RouteResponse>> GetById(int id)
    {
        try { return Ok(await service.FindRouteByIdAsync(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet("location/{location}")]
    public async Task<ActionResult<List<RouteResponse>>> ByLocation(string location)
        => Ok(await service.FindRoutesByLocationAsync(location));

    [HttpGet("country/{country}")]
    public async Task<ActionResult<List<RouteResponse>>> ByCountry(string country)
        => Ok(await service.FindRoutesByCountryAsync(country));

    [HttpGet("city/{city}")]
    public async Task<ActionResult<List<RouteResponse>>> ByCity(string city)
        => Ok(await service.FindRoutesByCityAsync(city));

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RouteResponse>> Update(int id, [FromBody] UpdateRouteRequest dto)
    {
        try { return Ok(await service.UpdateRouteAsync(id, dto)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteRouteAsync(id)) ? NoContent() : NotFound();
}