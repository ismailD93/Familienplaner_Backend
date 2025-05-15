using Microsoft.AspNetCore.Identity;

namespace KalenderAppBackend.Models;

public class AppUser : IdentityUser
{
    public int? CalendarId { get; set; }
    public virtual Calendar? Calendar { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public ICollection<UserEvent> UserEvents { get; set; } = [];
}