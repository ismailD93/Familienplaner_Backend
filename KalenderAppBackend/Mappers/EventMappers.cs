using KalenderAppBackend.Dtos.Event;
using KalenderAppBackend.Models;
using System.Runtime.CompilerServices;

namespace KalenderAppBackend.Mappers;

public static class EventMappers
{
    public static Event ToEventCreateDto(this CreateEventDto eventDto)
    {
        return new Event
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            StartDate = eventDto.StartDate,
            EndDate = eventDto.EndDate,
            IsDeleted = false,
        };
    }
}
