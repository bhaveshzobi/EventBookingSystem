namespace EventBookingSystem.Models
{
    public class EventBookingView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EventType Type { get; set; }
        public int TotalBookings { get; set; }
        public int TotalTickets { get; set; }
    }
}
