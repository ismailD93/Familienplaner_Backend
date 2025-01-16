using KalenderAppBackend.Data;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;

namespace KalenderAppBackend.Repos;

public class EventRepo : IEventRepo
{
    private readonly AppDbContext _context;

    public EventRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateAsync(Event eventModel)
    {
        await _context.Events.AddAsync(eventModel);
        await _context.SaveChangesAsync();

        return eventModel;
    }
}