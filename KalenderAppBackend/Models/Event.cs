namespace KalenderAppBackend.Models;

public class Event
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<UserEvent> UserEvents { get; set; } = [];
}
