using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace KalenderAppBackend.Interfaces;

public interface IAccountRepo
{   
    Task<AppUser?> GetUserByUserName(string userName);
}
