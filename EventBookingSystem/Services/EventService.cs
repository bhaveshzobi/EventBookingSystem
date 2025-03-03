// Services/EventService.cs
using EventBookingSystem.Data;
using EventBookingSystem.Models;
using EventBookingSystem.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EventBookingSystem.Services
{
    public sealed class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        private readonly IMemoryCache _cache;
        public event BookingNotificationHandler BookingCompleted;
        private const int PageSize = 5;

        public EventService(IEventRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public IQueryable<Event> GetAllEventsQuery(EventType? eventType = null)
        {
            return _repository.GetAllEventsQuery(eventType);
        }

        public IEnumerable<Event> GetAllEvents(EventType? eventType = null, int page = 1, int pageSize = 5)
        {
            string cacheKey = $"Events_{eventType?.ToString() ?? "All"}_{page}_{pageSize}";
            Console.WriteLine($"Checking cache for key: {cacheKey}");

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Event> cachedEvents))
            {
                Console.WriteLine("Cache miss, querying DB");
                cachedEvents = _repository.GetAllEvents(eventType, page, pageSize);
                _cache.Set(cacheKey, cachedEvents, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });
            }
            else
            {
                Console.WriteLine("Cache hit!");
            }
            return cachedEvents;
        }

        public Event GetEventById(int id)
        {
            return _repository.GetEventById(id);
        }

        public void CreateEvent(Event evt)
        {
            _repository.CreateEvent(evt);
        }

        public bool BookTickets(int eventId, int userId, int numberOfTickets)
        {
            Console.WriteLine($"Service: eventId={eventId}, userId={userId}, numberOfTickets={numberOfTickets}");
            try
            {
                _repository.BookTickets(eventId, userId, numberOfTickets, out int bookingId);
                if (bookingId > 0)
                {
                    var eventType = _repository.GetEventById(eventId)?.Type;
                    for (int page = 1; page <= 10; page++)
                    {
                        _cache.Remove($"Events_{eventType}_{page}_{PageSize}");
                        _cache.Remove($"Events_All_{page}_{PageSize}");
                    }
                    BookingCompleted?.Invoke(eventId, userId, numberOfTickets, bookingId);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service Error: {ex.Message}");
                throw;
            }
        }
    }
}