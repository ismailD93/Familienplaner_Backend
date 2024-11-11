using KalenderAppBackend.Dtos.Calendar;
using KalenderAppBackend.Models;


namespace KalenderAppBackend.Mappers;

public static class CalendarMappers
{
    public static CalendarDto ToCalendarDto(this Calendar calendarModel)
    {
        return new CalendarDto
        {
            Id = calendarModel.Id,
            Name = calendarModel.Name,
        };
    }

    public static Calendar ToCalendarCreateDto(this CreateCalendarDto calendarModel)
    {
        return new Calendar
        {
            Name = calendarModel.Name,
        };
    }
}
