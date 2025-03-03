using EventBookingSystem.Models;

namespace EventBookingSystem.Extensions
{
    public static class EventExtensions
    {
        public static string ToFormattedString(this Event evt)
        {
            return $"{evt.Name} on {evt.Date:MMMM dd, yyyy} at {evt.Venue} - ₹{evt.TicketPrice:F2}";
        }
    }
}