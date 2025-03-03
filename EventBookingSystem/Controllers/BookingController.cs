using EventBookingSystem.Data;
using EventBookingSystem.Models;
using EventBookingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly IEventService _eventService;
        private readonly AppDbContext _context;

        public BookingController(IEventService eventService,AppDbContext context)
        {
            _eventService = eventService;
            _context = context;
        }

        public IActionResult Create(int eventId)
        {
            var evt = _eventService.GetEventById(eventId);
            if (evt == null) return NotFound();

            var booking = new Booking { EventId = eventId, Event = evt };
            return View(booking);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    booking.UserId = userId;
                    bool success = _eventService.BookTickets(booking.EventId, userId, booking.NumberOfTickets);
                    if (success)
                    {
                        return RedirectToAction("Index", "Event");
                    }
                    ModelState.AddModelError("", "Not enough seats available.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error booking tickets: {ex.Message}");
            }
            booking.Event = _eventService.GetEventById(booking.EventId);
            return View(booking);
        }
        // Controllers/BookingController.cs
        [Authorize]
        [HttpGet]
        public IActionResult Dashboard()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var bookings = _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Event)
                .ToList();
            return View(bookings);
        }
    }
}