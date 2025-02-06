using KalenderAppBackend.Dtos.Event;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace KalenderAppBackend.Controllers;

public class UserEventController : ControllerBase
{
    private readonly IEventRepo _eventRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IUserEventRepo _userEventRepo;
    public UserEventController(IEventRepo eventRepo, IAccountRepo accountRepo, IUserEventRepo userEventRepo)
    {
        _eventRepo = eventRepo;
        _accountRepo = accountRepo; 
        _userEventRepo = userEventRepo;
    }

    [HttpPut("addUserEvent")]
    public async Task<IActionResult> Create([FromBody] CreateEventDto eventDto, string username)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var eventModel = eventDto.ToEventCreateDto();
        var user = await _accountRepo.GetUserByUserName(username);

        await _userEventRepo.CreateAsync(user, eventModel);

        return Ok(eventDto.ToEventCreateDto());
    }
}
