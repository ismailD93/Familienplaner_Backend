using Microsoft.AspNetCore.Identity;

namespace KalenderAppBackend.Models;

public class AppUser : IdentityUser
{
    public int? CalendarId { get; set; }
    public Calendar? Calendar { get; set; }
    public string? Color { get; set; }

    public ICollection<UserEvent> UserEvents { get; set; } = [];
}