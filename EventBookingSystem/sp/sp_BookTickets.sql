CREATE PROCEDURE sp_BookTickets
    @EventId INT,
    @UserId INT,
    @NumberOfTickets INT,
    @BookingId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @AvailableSeats INT;
        SELECT @AvailableSeats = AvailableSeats
        FROM [dbo].[Events]
        WHERE Id = @EventId;

        IF @AvailableSeats IS NULL
            THROW 50001, 'Event not found.', 1;
        IF @AvailableSeats < @NumberOfTickets
            THROW 50002, 'Not enough seats available.', 1;

        UPDATE [dbo].[Events]
        SET AvailableSeats = AvailableSeats - @NumberOfTickets
        WHERE Id = @EventId;

        INSERT INTO [dbo].[Bookings] (EventId, UserId, NumberOfTickets, BookingDate)
        VALUES (@EventId, @UserId, @NumberOfTickets, GETDATE());

        SET @BookingId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        THROW 50000, @ErrorMessage, 1;
    END CATCH
END
GO