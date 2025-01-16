using KalenderAppBackend.Models;

namespace KalenderAppBackend.Interfaces;

public interface IEventRepo
{
    Task<Event> CreateAsync(Event eventModel);
}
