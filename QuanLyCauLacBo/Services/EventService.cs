using Microsoft.EntityFrameworkCore;
using QuanLyCauLacBo.Data;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllEventsAsync(string? search = null, string? status = null, int? clubId = null)
        {
            var query = _context.Events
                .Include(e => e.Club)
                .Include(e => e.Registrations)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(e => e.Title.Contains(search) || (e.Description != null && e.Description.Contains(search)));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(e => e.Status == status);

            if (clubId.HasValue)
                query = query.Where(e => e.ClubId == clubId.Value);

            return await query.OrderByDescending(e => e.StartDate).ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
            => await _context.Events
                .Include(e => e.Club)
                .Include(e => e.CreatedBy)
                .Include(e => e.Registrations).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Event> CreateEventAsync(Event evt)
        {
            evt.CreatedAt = DateTime.Now;
            _context.Events.Add(evt);
            await _context.SaveChangesAsync();
            return evt;
        }

        public async Task<Event> UpdateEventAsync(Event evt)
        {
            _context.Events.Update(evt);
            await _context.SaveChangesAsync();
            return evt;
        }

        public async Task DeleteEventAsync(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt != null)
            {
                _context.Events.Remove(evt);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RegisterEventAsync(int eventId, int userId)
        {
            var evt = await _context.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == eventId);
            if (evt == null) return false;

            if (evt.MaxParticipants.HasValue && evt.Registrations.Count(r => r.Status != "Cancelled") >= evt.MaxParticipants.Value)
                return false;

            var existing = await _context.EventRegistrations
                .FirstOrDefaultAsync(er => er.EventId == eventId && er.UserId == userId);

            if (existing != null)
            {
                if (existing.Status == "Cancelled")
                {
                    existing.Status = "Registered";
                    existing.RegisteredAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            _context.EventRegistrations.Add(new EventRegistration
            {
                EventId = eventId,
                UserId = userId,
                Status = "Registered",
                RegisteredAt = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelRegistrationAsync(int eventId, int userId)
        {
            var reg = await _context.EventRegistrations
                .FirstOrDefaultAsync(er => er.EventId == eventId && er.UserId == userId);

            if (reg == null) return false;

            reg.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EventRegistration?> GetRegistrationAsync(int eventId, int userId)
            => await _context.EventRegistrations
                .FirstOrDefaultAsync(er => er.EventId == eventId && er.UserId == userId);

        public async Task<List<EventRegistration>> GetEventRegistrationsAsync(int eventId)
            => await _context.EventRegistrations
                .Include(er => er.User)
                .Where(er => er.EventId == eventId)
                .OrderBy(er => er.RegisteredAt)
                .ToListAsync();

        public async Task UpdateEventStatusesAsync()
        {
            var now = DateTime.Now;
            var events = await _context.Events
                .Where(e => e.Status != "Cancelled" && e.Status != "Completed")
                .ToListAsync();

            foreach (var evt in events)
            {
                if (now >= evt.StartDate && now <= evt.EndDate)
                    evt.Status = "Ongoing";
                else if (now > evt.EndDate)
                    evt.Status = "Completed";
            }

            await _context.SaveChangesAsync();
        }
    }
}
