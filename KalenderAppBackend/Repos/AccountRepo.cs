using KalenderAppBackend.Data;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KalenderAppBackend.Repos;

public class AccountRepo : IAccountRepo
{
    private readonly AppDbContext _context;
    public AccountRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> GetUserByUserName(string userName)
    {
        AppUser? user = (AppUser?)await _context.Users.Cast<AppUser>().Include(u => u.Calendar).FirstOrDefaultAsync(x => x.UserName == userName);

        return user;
    }
}
