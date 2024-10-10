namespace KalenderAppBackend.Models;

public class Calendar
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<AppUser> FamilyMembers { get; set; } = [];
}
