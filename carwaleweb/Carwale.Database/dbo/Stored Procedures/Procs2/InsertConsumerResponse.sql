IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertConsumerResponse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertConsumerResponse]
GO
	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR ConsumerResponse  TABLE

CREATE PROCEDURE [dbo].[InsertConsumerResponse]
	@PriceRange		VARCHAR(50),
	@Response		INT,
	@AvailableCars		INT,
	@IsDealerResponse	BIT,
	@EntryDate		DATETIME
	

 AS
	BEGIN
		SELECT Id FROM ConsumerResponse WHERE EntryDate = @EntryDate AND IsDealerResponse = @IsDealerResponse AND  PriceRange = @PriceRange
		
		IF @@ROWCOUNT =  0
			BEGIN
				INSERT INTO ConsumerResponse
				(
					PriceRange, Response, AvailableCars, IsDealerResponse, EntryDate
				)
				VALUES
				(
					@PriceRange, @Response,@AvailableCars, @IsDealerResponse, @EntryDate
				)
			END
		ELSE
			BEGIN
				UPDATE ConsumerResponse SET 
					PriceRange = @PriceRange, Response = @Response, 
					AvailableCars = @AvailableCars, IsDealerResponse = @IsDealerResponse, 
					EntryDate = @EntryDate
				WHERE  EntryDate = @EntryDate AND IsDealerResponse = @IsDealerResponse AND  PriceRange = @PriceRange
			END
	END
