using KalenderAppBackend.Data;
using KalenderAppBackend.Dtos.Event;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace KalenderAppBackend.Repos;

public class EventRepo : IEventRepo
{
    private readonly AppDbContext _context;
    private readonly ICalendarRepo _calendarRepo;

    public EventRepo(AppDbContext context, ICalendarRepo calendarRepo)
    {
        _context = context;
        _calendarRepo = calendarRepo;
    }

    public async Task<Event> CreateAsync(Event eventModel)
    {
        await _context.Events.AddAsync(eventModel);
        await _context.SaveChangesAsync();

        return eventModel;
    }

    public async Task<List<EventDto>> GetAllEventsFromCalendarAsync(int calendarId)
    {
        var familyMembers = await _calendarRepo.GetAllFamilyMembers(calendarId);

        var list = new List<EventDto>();

        foreach (var familyMember in familyMembers)
        {
            var userEvents = await _context.UserEvent
                .Include(ue => ue.Event)
                .Where(ue => ue.UserId == familyMember.Id)
                .Select(ue => ue.Event)
                .ToListAsync();

            list.AddRange(userEvents.Select(e => new EventDto
            {
                Id = e.Id,
                UserId = familyMember.Id,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
            }));
        }

        return list;
    }

    public Task<Event?> UpdateAsync(UpdateEventDto eventModel, int id)
    {
        var eventToUpdate = _context.Events.FirstOrDefault(e => e.Id == id);
        if (eventToUpdate == null)
        {
            return Task.FromResult<Event?>(null);
        }
        if (eventModel.Title != null)
        {
            eventToUpdate.Title = eventModel.Title;
        }
        if (eventModel.Description != null)
        {
            eventToUpdate.Description = eventModel.Description;
        }
        if (eventModel.IsDeleted)
        {
            eventToUpdate.IsDeleted = true;
        }
        _context.Events.Update(eventToUpdate);
        _context.SaveChanges();
        return Task.FromResult<Event?>(eventToUpdate);
    }
}