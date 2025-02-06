using Microsoft.AspNetCore.Identity;

namespace KalenderAppBackend.Models;

//zwischentabelle um n zu n verhindern
public class UserEvent
{
    public required string UserId { get; set; }
    public required AppUser User { get; set; }

    public required int EventId { get; set; }
    public required Event Event { get; set; }
}
