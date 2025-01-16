using KalenderAppBackend.Controllers;
using KalenderAppBackend.Dtos.Calendar;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KalenderAppBackend.Tests;

public class CalendarControllerTests
{
    private readonly CalendarController _controller;
    private readonly Mock<ICalendarRepo> _mockCalendarRepo;

    public CalendarControllerTests()
    {
        _mockCalendarRepo = new Mock<ICalendarRepo>();

        _controller = new CalendarController(_mockCalendarRepo.Object);
    }

    [Fact]
    public async Task Test_BadRequest_GetAll()
    {
        _controller.ModelState.AddModelError("error", "i am a fake error");

        var result = await _controller.GetAll();

        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task Test_Successful_GetAll()
    {
        var calendars = new List<Calendar> { new Calendar { Name = "test" } };

        _mockCalendarRepo
            .Setup(cr => cr.GetAllAsync())
            .ReturnsAsync(calendars);

        var result = await _controller.GetAll();

        var okresult = Assert.IsType<OkObjectResult>(result);
        var calendarDto = Assert.IsType<List<CalendarDto>>(okresult.Value);
        Assert.Equal(calendars.First().Name, calendarDto.First().Name);
    }

    [Fact]
    public async Task Test_Successful_GetById()
    {
        Calendar calendar = new() { Id = 1, Name = "test" };

        _mockCalendarRepo
            .Setup(cr => cr.GetByIdAsync(calendar.Id))
            .ReturnsAsync(calendar);

        var result = await _controller.GetById(calendar.Id);

        var okObject = Assert.IsType<OkObjectResult>(result);
        var calendarDto = Assert.IsType<CalendarDto>(okObject.Value);
        Assert.Equal(calendar.Id, calendarDto.Id);
        Assert.Equal(calendar.Name, calendarDto.Name);
    }
    [Fact]
    public async Task Test_BadRequest_GetById()
    {
        _controller.ModelState.AddModelError("error", "this is a fake error");

        var result = await _controller.GetById(1);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task Test_BadRequest_NonExistingId_GetById()
    {
        var result = await _controller.GetById(-5);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Test_Successful_Create()
    {
        CreateCalendarDto createCalendarDto = new() { Name = "test" };

        _mockCalendarRepo
            .Setup(cr => cr.CreateAsync(It.IsAny<Calendar>()))
            .ReturnsAsync(new Calendar() { Name = "test" });

        var result = await _controller.Create(createCalendarDto);

        var okObject = Assert.IsType<OkObjectResult>(result);
        var calendarDto = Assert.IsType<CalendarDto>(okObject.Value);
        Assert.Equal(createCalendarDto.Name, calendarDto.Name);
    }
    [Fact]
    public async Task Test_BadRequest_Create()
    {
        _controller.ModelState.AddModelError("error", "this is a fake error");

        var result = await _controller.Create(new CreateCalendarDto { Name = "test" });

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
