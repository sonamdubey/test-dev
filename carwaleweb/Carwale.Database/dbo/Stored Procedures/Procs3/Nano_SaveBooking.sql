IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Nano_SaveBooking]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Nano_SaveBooking]
GO

	
CREATE PROCEDURE [dbo].[Nano_SaveBooking]
	@CustomerId			NUMERIC,	
	@Name				VARCHAR(100),
	@Email				VARCHAR(50),	
	@MobileNo			VARCHAR(50),
	@LandlineNo			VARCHAR(50),
	@CityId				NUMERIC,	
	@Address			VARCHAR(500),
	@VersionId			NUMERIC,
	@BookingAmount		NUMERIC,
	@StampDuty			NUMERIC,
	@PaymentMode		SMALLINT,
	@RequestDateTime	DATETIME,
	@SourceId			SMALLINT,
	@Id					NUMERIC OUTPUT
	
 AS
	DECLARE @InvNo AS VarChar(50)
BEGIN
	
		INSERT INTO Nano_Booking
		(
			CustomerId, Name, Email, MobileNo, LandlineNo,
			CityId, Address, VersionId, BookingAmount,
			StampDuty, PaymentMode, RequestDateTime, SourceId
		)
		VALUES
		(  
			@CustomerId, @Name, @Email, @MobileNo, @LandlineNo,
			@CityId, @Address, @VersionId, @BookingAmount,
			@StampDuty, @PaymentMode, @RequestDateTime, @SourceId
		)
		
		SET @Id = SCOPE_IDENTITY()
	
		SET @InvNo = 'CWNB/' + Convert(VarChar(50), Year(@RequestDateTime)) + '/' + Convert(VarChar(50), Month(@RequestDateTime)) + '/' + Convert(VarChar(50), Day(@RequestDateTime)) + '/' + Convert(VarChar(50), @Id)
		
		UPDATE Nano_Booking SET InvNo = @InvNo WHERE Id = @Id

END

