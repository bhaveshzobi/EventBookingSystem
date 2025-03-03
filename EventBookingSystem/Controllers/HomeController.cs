using EventBookingSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var eventCount = _context.Events.Count();
            ViewBag.Message = $"Database has {eventCount} events.";
            return View();
        }
    }
}
