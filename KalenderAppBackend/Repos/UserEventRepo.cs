using KalenderAppBackend.Data;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;

namespace KalenderAppBackend.Repos;

public class UserEventRepo : IUserEventRepo
{
    private readonly AppDbContext _context;

    public UserEventRepo(AppDbContext context)
    {
        _context = context;
    }

    public Task<UserEvent> CreateAsync(string userName, int CalendarId)
    {
        
    }
}
