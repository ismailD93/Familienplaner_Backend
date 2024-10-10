using KalenderAppBackend.Models;

namespace KalenderAppBackend.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
