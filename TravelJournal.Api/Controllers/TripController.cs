using Microsoft.AspNetCore.Mvc;
using TravelJournal.Application.DTOs.Input;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;

namespace TravelJournal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripController(ITripService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TripResponse>> Create([FromBody] CreateTripRequest dto)
    {
        var result = await service.CreateTripAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<TripResponse>>> GetAll()
        => Ok(await service.FindAllTripsAsync());

    [HttpGet("search")]
    public async Task<ActionResult<List<TripResponse>>> Search([FromQuery] string title)
        => Ok(await service.FindTripsByTitleAsync(title));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TripResponse>> GetById(int id)
    {
        try { return Ok(await service.FindTripByIdAsync(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TripResponse>> Update(int id, [FromBody] UpdateTripRequest dto)
    {
        try { return Ok(await service.UpdateTripAsync(id, dto)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteTripAsync(id)) ? NoContent() : NotFound();
}
