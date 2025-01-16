using KalenderAppBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KalenderAppBackend.Controllers;

[Route("api/event")]
[ApiController]
public class EventController
{
    private readonly IEventRepo _eventRepo;

    public EventController(IEventRepo eventRepo)
    {
        _eventRepo = eventRepo;
    }
}
