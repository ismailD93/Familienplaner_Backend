using Microsoft.AspNetCore.Identity;

namespace KalenderAppBackend.Models;

public class AppUser : IdentityUser
{
    public int? CalendarId { get; set; }
    public virtual Calendar? Calendar { get; set; }

    public ICollection<UserEvent> UserEvents { get; set; } = [];
}