namespace EventBookingSystem.Models
{
    public class Booking
    {
        // Default constructor
        public Booking() { }

        // Parameterized constructor
        public Booking(int eventId, int userId, int numberOfTickets)
        {
            EventId = eventId;
            UserId = userId;
            NumberOfTickets = numberOfTickets;
            BookingDate = DateTime.Now; // Set booking date to now
        }

        public int Id { get; set; }
        public int EventId { get; set; }
        public int? UserId { get; set; }
        public int NumberOfTickets { get; set; }
        public DateTime BookingDate { get; set; }
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
    }
}