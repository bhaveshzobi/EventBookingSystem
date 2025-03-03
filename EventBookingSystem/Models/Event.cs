// Models/Event.cs
namespace EventBookingSystem.Models
{
    public enum EventType { Concert, Conference, Workshop }

    public partial class Event
    {
        // Default constructor
        public Event() { }

        // Parameterized constructor
        public Event(string name, DateTime date, string venue, int totalSeats, EventType type, decimal ticketPrice)
        {
            Name = name;
            Date = date;
            Venue = venue;
            TotalSeats = totalSeats;
            AvailableSeats = totalSeats; // Initially all seats available
            Type = type;
            TicketPrice = ticketPrice;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public EventType Type { get; set; }
        public decimal TicketPrice { get; set; }
    }

    public partial class Event
    {
        public bool IsValid() => !string.IsNullOrEmpty(Name) && TotalSeats > 0 && TicketPrice >= 0;
    }
}