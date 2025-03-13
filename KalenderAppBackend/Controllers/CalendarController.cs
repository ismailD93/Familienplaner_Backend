using KalenderAppBackend.Dtos.Calendar;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace KalenderAppBackend.Controllers;

[Route("api/calendar")]
[ApiController]
public class CalendarController : ControllerBase
{
    private readonly ICalendarRepo _calendarRepo;
    public CalendarController(ICalendarRepo calendarRepo)
    {
        _calendarRepo = calendarRepo;
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var calendars = await _calendarRepo.GetAllAsync();

        var calendarDto = calendars.Select(c => c.ToCalendarDto()).ToList();

        return Ok(calendarDto);
    }

    [HttpGet("getBy{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var calendar = await _calendarRepo.GetByIdAsync(id);

        if (calendar == null)
            return BadRequest("id not found");

        return Ok(calendar.ToCalendarDto());
    }

    [HttpGet("getByName")]
    public async Task<IActionResult> GetByName(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var calendar = await _calendarRepo.GetCalendarByName(name);

        if (calendar == null)
            return BadRequest("no calendar found with this name");

        return Ok(calendar.ToCalendarDto());
    }

    [HttpPut("addCalendar")]
    public async Task<IActionResult> Create([FromBody] CreateCalendarDto calendarDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var calendarModel = calendarDto.ToCalendarCreateDto();
        await _calendarRepo.CreateAsync(calendarModel);

        return Ok(calendarModel.ToCalendarDto());
    }

}
