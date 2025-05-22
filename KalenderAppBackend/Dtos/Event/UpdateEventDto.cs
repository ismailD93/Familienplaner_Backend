namespace KalenderAppBackend.Dtos.Event;

public class UpdateEventDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
}
