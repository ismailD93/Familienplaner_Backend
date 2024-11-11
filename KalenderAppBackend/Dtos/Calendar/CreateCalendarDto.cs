using System.ComponentModel.DataAnnotations;

namespace KalenderAppBackend.Dtos.Calendar;

public class CreateCalendarDto
{
    [Required]
    [MaxLength(20, ErrorMessage = "Symbol cannot be over 20 characters")]
    public required string Name { get; set; }
}
