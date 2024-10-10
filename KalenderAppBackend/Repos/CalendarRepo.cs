using KalenderAppBackend.Data;
using KalenderAppBackend.Interfaces;

namespace KalenderAppBackend.Repos;

public class CalendarRepo : ICalendarRepo
{
    private readonly AppDbContext _context;

    public CalendarRepo(AppDbContext context)
    {
        _context = context;
    }
}
