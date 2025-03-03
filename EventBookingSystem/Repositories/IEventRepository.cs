// Repositories/IEventRepository.cs
using EventBookingSystem.Models;
using System.Linq;

namespace EventBookingSystem.Repositories
{
    public interface IEventRepository
    {
        IQueryable<Event> GetAllEventsQuery(EventType? eventType = null);
        IEnumerable<Event> GetAllEvents(EventType? eventType = null, int page = 1, int pageSize = 5);
        Event GetEventById(int id);
        void CreateEvent(Event evt);
        void BookTickets(int eventId, int userId, int numberOfTickets, out int bookingId);
    }
}