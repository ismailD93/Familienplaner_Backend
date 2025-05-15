using KalenderAppBackend.Dtos.Calendar;
using KalenderAppBackend.Models;


namespace KalenderAppBackend.Mappers;

public static class Calendarmappers
{
    public static FamilyMemberDto ToFamilyMemberDto(this AppUser familyMember)
    {
        return new FamilyMemberDto
        {
            Id = familyMember.Id,
            Name = familyMember.UserName,
            CalendarId = familyMember.CalendarId ?? 0,
            Color = familyMember.Color ?? string.Empty,
        };
    }

    public static CalendarDto ToCalendarDto(this Calendar calendarModel)
    {
        return new CalendarDto
        {
            Id = calendarModel.Id,
            Name = calendarModel.Name,
            FamilyMembers = calendarModel.FamilyMembers?.Select(ToFamilyMemberDto).ToList(),
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
