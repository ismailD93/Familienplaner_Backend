using System.ComponentModel.DataAnnotations;

namespace KalenderAppBackend.Dtos.Event;

public class CreateEventDto
{
    [Required]
    [MaxLength(20, ErrorMessage = "Symbol cannot be over 20 characters")]
    public required string Title { get; set; }
    [Required]
    [MaxLength(20, ErrorMessage = "Symbol cannot be over 20 characters")]
    public required string Description { get; set; }
    [Required]
    public required DateTimeOffset StartDate { get; set; }
    [Required]
    public required DateTimeOffset EndDate { get; set; }
}
