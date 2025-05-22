using KalenderAppBackend.Data;
using KalenderAppBackend.Dtos.Calendar;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace KalenderAppBackend.Repos;

public class CalendarRepo : ICalendarRepo
{
    private readonly AppDbContext _context;

    public CalendarRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Calendar>> GetAllAsync()
    {
        List<Calendar> calendars = await _context.Calendars.Include(fm => fm.FamilyMembers).ToListAsync();

        return calendars;
    }
    public async Task<Calendar?> GetByIdAsync(int id)
    {
        return await _context.Calendars.Include(fm => fm.FamilyMembers).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<Calendar> CreateAsync(Calendar calendarModel)
    {
        await _context.Calendars.AddAsync(calendarModel);
        await _context.SaveChangesAsync();
        return calendarModel;
    }
    public async Task<Calendar?> UpdateAsync(int id, UpdateCalendarDto calendarDto)
    {
        var existingCalendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id == id);

        if (existingCalendar == null)
            return null;

        if (calendarDto.Name != null)
            existingCalendar.Name = calendarDto.Name;
        if (calendarDto.IsDeleted)
            existingCalendar.IsDeleted = calendarDto.IsDeleted;

        await _context.SaveChangesAsync();

        return existingCalendar;
    }
    public async Task<Calendar?> DeleteAsync(int id)
    {
        var calendarModel = await _context.Calendars.FirstOrDefaultAsync(x => x.Id == id);

        if (calendarModel == null)
            return null;

        _context.Calendars.Remove(calendarModel);
        await _context.SaveChangesAsync();
        return calendarModel;
    }

    public async Task<AppUser?> AddFamilyMember(int calendarId, string userId)
    {
        Calendar? calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id == calendarId);

        if (calendar == null)
            return null;

        AppUser? user = calendar.FamilyMembers.FirstOrDefault(x => x.Id == userId);

        if (user == null)
            return null;

        calendar.FamilyMembers.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    public async Task<AppUser?> RemoveFamilyMember(int calendarId, string userId)
    {
        Calendar? calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id == calendarId);

        if (calendar == null)
            return null;

        AppUser? user = calendar.FamilyMembers.FirstOrDefault(x => x.Id == userId);

        if (user == null)
            return null;

        calendar.FamilyMembers.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<List<AppUser>> GetAllFamilyMembers(int calendarId)
    {
        var familyMembers = _context.Users
            .OfType<AppUser>()
            .Include(u => u.UserEvents)
                .ThenInclude(ue => ue.Event)
                .ToList();

        return familyMembers;
    }

    public async Task<Calendar?> GetCalendarByName(string name)
    {
        var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Name == name);

        if (calendar == null)
            return null;

        return calendar;
    }
}