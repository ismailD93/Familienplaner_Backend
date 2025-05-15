namespace KalenderAppBackend.Dtos.Account;

public class GetUserDto
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Color { get; set; }
}
