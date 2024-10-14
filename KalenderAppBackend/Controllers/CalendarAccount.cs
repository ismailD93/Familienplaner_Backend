using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace KalenderAppBackend.Controllers;

[Route("api/calendar")]
[ApiController]
public class CalendarAccount : ControllerBase
{
    private readonly ICalendarRepo _calendarRepo;
    public CalendarAccount(ICalendarRepo calendarRepo)
    {
        _calendarRepo = calendarRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var calendars = await _calendarRepo.GetAllAsync();

        var calendarDto = calendars.Select(c => c.ToCalendarDto()).ToList();

        return Ok(calendarDto);
    }
}
