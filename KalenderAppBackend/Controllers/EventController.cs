using KalenderAppBackend.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KalenderAppBackend.Controllers;

[Route("api/event")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventRepo _eventRepo;
    public EventController(IEventRepo eventRepo)
    {
        _eventRepo = eventRepo;
    }

    [HttpGet("getAllEventsFromCalendar")]
    public async Task<IActionResult> GetAllEventsFromCalendar(int calendarId)
    {
        var result = await _eventRepo.GetAllEventsFromCalendarAsync(calendarId);

        if (result == null)
            return NotFound(result);

        return Ok(result);
    }
}
