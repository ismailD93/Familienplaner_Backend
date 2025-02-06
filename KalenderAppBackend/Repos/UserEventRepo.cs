using KalenderAppBackend.Data;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace KalenderAppBackend.Repos;

public class UserEventRepo : IUserEventRepo
{
    private readonly AppDbContext _context;

    public UserEventRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserEvent> CreateAsync(AppUser user, Event @event)
    {
        UserEvent userEventModel = new UserEvent {
            User = user,
            UserId = user.Id,

            Event = @event,
            EventId = @event.Id,
        };

        await _context.UserEvent.AddAsync(userEventModel);
        await _context.SaveChangesAsync();

        return userEventModel;
    }
}
