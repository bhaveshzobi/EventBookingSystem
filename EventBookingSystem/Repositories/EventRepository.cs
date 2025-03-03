// Repositories/EventRepository.cs
using EventBookingSystem.Data;
using EventBookingSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Event> GetAllEventsQuery(EventType? eventType = null)
        {
            var eventsQuery = _context.Events.AsQueryable();
            if (eventType.HasValue)
            {
                eventsQuery = eventsQuery.Where(e => e.Type == eventType.Value);
            }
            return eventsQuery;
        }

        public IEnumerable<Event> GetAllEvents(EventType? eventType = null, int page = 1, int pageSize = 5)
        {
            return GetAllEventsQuery(eventType)
                .OrderBy(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Event GetEventById(int id)
        {
            return _context.Events.FirstOrDefault(e => e.Id == id);
        }

        public void CreateEvent(Event evt)
        {
            if (evt == null || !evt.IsValid()) throw new ArgumentException("Invalid event data");
            _context.Events.Add(evt);
            _context.SaveChanges();
        }

        public void BookTickets(int eventId, int userId, int numberOfTickets, out int bookingId)
        {
            var bookingIdParam = new SqlParameter("@BookingId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_BookTickets @EventId = {0}, @UserId = {1}, @NumberOfTickets = {2}, @BookingId = {3} OUTPUT",
                eventId, userId, numberOfTickets, bookingIdParam);

            bookingId = bookingIdParam.Value != DBNull.Value ? Convert.ToInt32(bookingIdParam.Value) : 0;
        }
    }
}