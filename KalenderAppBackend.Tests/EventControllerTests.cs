using KalenderAppBackend.Controllers;
using KalenderAppBackend.Interfaces;
using Moq;

namespace KalenderAppBackend.Tests;

public class EventControllerTests
{
    private readonly EventController _controller;
    private readonly Mock<IEventRepo> _mockcalendarRepo;

    public EventControllerTests()
    {
        _mockcalendarRepo = new Mock<IEventRepo>();

        _controller = new EventController(_mockcalendarRepo.Object);
    }

    [Fact]
    public async Task Test_BadRequest_GetAllFromUser()
    {

    }
}
