using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync(string? search = null, string? status = null, int? clubId = null);
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event> CreateEventAsync(Event evt);
        Task<Event> UpdateEventAsync(Event evt);
        Task DeleteEventAsync(int id);
        Task<bool> RegisterEventAsync(int eventId, int userId);
        Task<bool> CancelRegistrationAsync(int eventId, int userId);
        Task<EventRegistration?> GetRegistrationAsync(int eventId, int userId);
        Task<List<EventRegistration>> GetEventRegistrationsAsync(int eventId);
        Task UpdateEventStatusesAsync();
    }
}
