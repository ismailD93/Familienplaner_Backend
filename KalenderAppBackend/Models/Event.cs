namespace KalenderAppBackend.Models;

public class Event
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public ICollection<UserEvent> UserEvents { get; set; } = [];
}
