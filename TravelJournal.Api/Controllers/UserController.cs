using Microsoft.AspNetCore.Mvc;
using TravelJournal.Application.DTOs.Output;
using TravelJournal.Application.Interfaces;

namespace TravelJournal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById([FromRoute] int id)
    {
        try
        {
            var user = await service.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id:int}/trips")]
    public async Task<ActionResult<List<TripResponse>>> GetUserTrips([FromRoute] int id)
    {
        try
        {
            var trips = await service.GetUserTripsAsync(id);
            return Ok(trips);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
} 