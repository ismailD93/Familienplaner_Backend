using KalenderAppBackend.Dtos.Event;
using KalenderAppBackend.Models;

namespace KalenderAppBackend.Interfaces;

public interface IEventRepo
{
    Task<Event> CreateAsync(Event eventModel);
    Task<List<EventDto>> GetAllEventsFromCalendarAsync(int calendarId);
}
