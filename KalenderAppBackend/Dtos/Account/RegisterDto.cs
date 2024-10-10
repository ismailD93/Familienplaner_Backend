using System.ComponentModel.DataAnnotations;
namespace KalenderAppBackend.Dtos.Account;

public class RegisterDto
{
    public required string UserName { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}
