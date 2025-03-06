namespace KalenderAppBackend.Dtos.Calendar;

public class FamilyMemberDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public int? CalendarId { get; set; }
}
