// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace EventBookingSystem.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FullName { get; set; }
    }
}