namespace KalenderAppBackend.Dtos.Event;

public class EventDto
{
    public required string UserId { get; set; }
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public required DateTimeOffset EndDate { get; set; }
}
