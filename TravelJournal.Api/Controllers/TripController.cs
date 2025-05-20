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
    public async Task<ActionResult<TripResponse>> Create([FromBody] CreateTripRequest request)
    {
        try 
        {
            var created = await service.CreateTaskAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<TripResponse>>> GetAll()
    {
        var list = await service.FindAllTasksAsync();
        return Ok(list);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<TripResponse>>> Search([FromQuery] string title)
    {
        var list = await service.FindTasksByTitleAsync(title);
        return Ok(list);
    }
    
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<List<TripResponse>>> GetByUserId([FromRoute] int userId)
    {
        try
        {
            var trips = await service.FindTasksByUserIdAsync(userId);
            return Ok(trips);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TripResponse>> GetById([FromRoute] int id)
    {
        try
        {
            var trip = await service.FindTaskByIdAsync(id);
            return Ok(trip);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TripResponse>> Update(
        [FromRoute] int id, 
        [FromBody] UpdateTripRequest request)
    {
        try
        {
            var updated = await service.UpdateTaskAsync(id, request);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var removed = await service.DeleteTaskAsync(id);
        if (!removed)
            return NotFound();

        return NoContent();
    }
}
