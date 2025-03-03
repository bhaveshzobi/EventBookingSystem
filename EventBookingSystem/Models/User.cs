// Models/User.cs
namespace EventBookingSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        private string _passwordHash; // Encapsulation
        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = value;
        }
    }
}