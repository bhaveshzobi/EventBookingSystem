CREATE VIEW vw_EventBookings AS
SELECT e.Id, e.Name, e.Type, COUNT(b.Id) AS TotalBookings, SUM(b.NumberOfTickets) AS TotalTickets
FROM Events e
LEFT JOIN Bookings b ON e.Id = b.EventId
GROUP BY e.Id, e.Name, e.Type;