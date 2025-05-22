using System.ComponentModel.DataAnnotations;

namespace KalenderAppBackend.Dtos.Calendar;

public class UpdateCalendarDto
{
    [MaxLength(25, ErrorMessage = "Name cannot be over 25 characters")]
    public required string Name { get; set; }
    public bool IsDeleted { get; set; }
}
