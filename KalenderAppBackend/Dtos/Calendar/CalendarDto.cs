using KalenderAppBackend.Models;

namespace KalenderAppBackend.Dtos.Calendar;

public class CalendarDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<FamilyMemberDto> FamilyMembers { get; set; }
}
