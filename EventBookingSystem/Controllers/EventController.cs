using EventBookingSystem.Services;
using EventBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EventBookingSystem.Extensions;
using Microsoft.EntityFrameworkCore;
using EventBookingSystem.Data;

namespace EventBookingSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly AppDbContext _context;

        public EventController(IEventService eventService, AppDbContext dbContext)
        {
            _eventService = eventService;
            _context = dbContext;
            
            _eventService.BookingCompleted += OnBookingCompleted;
        }
        private void OnBookingCompleted(int eventId, int userId, int numberOfTickets, int bookingId)
        {
            
            Console.WriteLine($"Booking Notification: User {userId} booked {numberOfTickets} tickets for Event {eventId}. Booking ID: {bookingId}");
        }

        // Controllers/EventController.cs
        [HttpGet]
        public IActionResult Index([FromQuery] EventType? eventType = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var events = _eventService.GetAllEvents(eventType, page, pageSize);
            var totalEvents = eventType.HasValue ? _context.Events.Count(e => e.Type == eventType.Value) : _context.Events.Count();
            var totalPages = (int)Math.Ceiling(totalEvents / (double)pageSize);
            ViewBag.EventType = eventType;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            return View(events);
        }

        [HttpGet]
        public IActionResult GetEvents([FromQuery] EventType? eventType = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                Console.WriteLine($"GetEvents called with eventType: {eventType}, page: {page}, pageSize: {pageSize}");
                var events = _eventService.GetAllEvents(eventType, page, pageSize).Select(e => new
                {
                    e.Id,
                    Name = e.ToFormattedString(),
                    Date = e.Date.ToShortDateString(),
                    e.Venue,
                    Type = e.Type.ToString(),
                    e.AvailableSeats,
                    BookUrl = Url.Action("Create", "Booking", new { eventId = e.Id })
                }).ToList();

                var totalEvents = eventType.HasValue
                    ? _context.Events.Count(e => e.Type == eventType.Value)
                    : _context.Events.Count();
                var totalPages = (int)Math.Ceiling(totalEvents / (double)pageSize);

                Console.WriteLine($"Returning {events.Count} events, Total Pages: {totalPages}");
                return Json(new { events, totalPages, currentPage = page });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching events: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult BookTickets([FromBody] BookingRequest request)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                bool success = _eventService.BookTickets(request.EventId, userId, request.NumberOfTickets);
                if (success)
                {
                    var updatedEvent = _eventService.GetEventById(request.EventId);
                    return Json(new { success = true, availableSeats = updatedEvent.AvailableSeats });
                }
                return Json(new { success = false, message = "Not enough seats available." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event newEvent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _eventService.CreateEvent(newEvent);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating event: {ex.Message}");
            }
            return View(newEvent);
        }
        // Controllers/EventController.cs
        [HttpGet]
        public IActionResult Details(int id)
        {
            var evt = _eventService.GetEventById(id);
            if (evt == null) return NotFound();
            return View(evt);
        }
    }
}