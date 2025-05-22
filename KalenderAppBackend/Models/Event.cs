namespace KalenderAppBackend.Models;

public class Event
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public required DateTimeOffset EndDate { get; set; }
    public required bool IsDeleted { get; set; } = false;

    public ICollection<UserEvent> UserEvents { get; set; } = [];
}
