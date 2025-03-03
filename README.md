# Event Booking System

A robust web application built with ASP.NET Core MVC for managing events and bookings. This project showcases a blend of backend and frontend development skills, delivering a secure, efficient, and user-friendly experience.

## Features
- **Authentication**: Secure user login/registration using ASP.NET Core Identity with custom `ApplicationUser` (int keys).
- **Event Management**: Create, view, and manage events with full CRUD operations via Entity Framework Core.
- **Booking System**: Real-time ticket booking with a stored procedure (`sp_BookTickets`), ensuring atomic updates.
- **Performance**: In-memory caching (`IMemoryCache`) for event lists, reducing database load.
- **UI/UX**: 
  - Responsive design with Bootstrap.
  - Pagination and filtering for event lists.
  - Dark mode toggle with local storage persistence.
  - Hover effects and fade animations for seat updates.
  - Event details page and user dashboard for personalized views.
- **Data Access**: Repository pattern for cleaner separation, SQL views for booking stats, and LINQ for querying.
- **Extras**: Delegates for booking notifications, partial views for reusable UI, and modern JavaScript (async/await).

## Technologies
- **Backend**: C#, .NET Core MVC, Entity Framework Core, SQL Server
- **Frontend**: HTML, CSS (Bootstrap), JavaScript (jQuery, Fetch API)
- **Patterns**: Repository, SOLID principles (e.g., Single Responsibility)
- **Features**: LINQ, Delegates, Caching, Authentication

## Achievements
- Implemented all requested features from the topic list (e.g., Sealed Class, StringBuilder, JOINS).
- Added bonus enhancements (Event Details, User Dashboard, Dark Mode).
- Ensured scalability and performance with caching and efficient queries.

## How to Run
1. Clone the repository.
2. Set up SQL Server with `EventBookingDb` and run migrations (`dotnet ef database update`).
3. Configure connection string in `appsettings.json`.
4. Run: `dotnet run`—navigate to `https://localhost:<port>/Event`.

Built with a focus on learning and practical application—enjoy exploring events!
