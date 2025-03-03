// Services/IEventService.cs
using EventBookingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventBookingSystem.Services
{
    public delegate void BookingNotificationHandler(int eventId, int userId, int numberOfTickets, int bookingId);

    public interface IEventService
    {
        IQueryable<Event> GetAllEventsQuery(EventType? eventType = null);
        IEnumerable<Event> GetAllEvents(EventType? eventType = null, int page = 1, int pageSize = 5);
        Event GetEventById(int id);
        void CreateEvent(Event evt);
        bool BookTickets(int eventId, int userId, int numberOfTickets);
        event BookingNotificationHandler BookingCompleted;
    }
}