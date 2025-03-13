using KalenderAppBackend.Dtos.Calendar;
using KalenderAppBackend.Models;

namespace KalenderAppBackend.Interfaces;

public interface ICalendarRepo
{
    Task<List<Calendar>> GetAllAsync();
    Task<Calendar?> GetByIdAsync(int id);
    Task<Calendar> CreateAsync(Calendar calendarModel);
    Task<Calendar?> UpdateAsync(int id, UpdateCalendarDto calendarDto);
    Task<Calendar?> DeleteAsync(int id);
    Task<AppUser?> AddFamilyMember(int calendarId, string userId);
    Task<AppUser?> RemoveFamilyMember(int calendarId, string userId);
    Task<List<AppUser>> GetAllFamilyMembers(int calendarId);
    Task<Calendar?> GetCalendarByName(string name);
}
