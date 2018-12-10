IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntrySaveUsedCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntrySaveUsedCar]
GO

	
--THIS PROCEDURE IS FOR EntryCustomerFavourites

CREATE PROCEDURE [dbo].[EntrySaveUsedCar]
	@CustomerId		NUMERIC, 
	@CarProfileId		VARCHAR(50), 
	@EntryDateTime	DATETIME
	
 AS
	
BEGIN
	
	--IT IS FOR THE INSERT
	
	SELECT Id FROM CustomerFavouritesUsed WHERE CustomerId=@CustomerId AND CarProfileId = @CarProfileId

	IF @@ROWCOUNT = 0
		INSERT INTO CustomerFavouritesUsed
			(
				CustomerId, 		CarProfileId, 		EntryDateTime
			)
			VALUES
			(	
				@CustomerId, 		@CarProfileId, 		@EntryDateTime
			)

		
END
