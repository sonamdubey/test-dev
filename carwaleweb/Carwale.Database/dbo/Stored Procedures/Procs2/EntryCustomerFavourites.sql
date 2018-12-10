IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryCustomerFavourites]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryCustomerFavourites]
GO

	
--THIS PROCEDURE IS FOR EntryCustomerFavourites

CREATE PROCEDURE [dbo].[EntryCustomerFavourites]
	@CustomerId		NUMERIC, 
	@VersionId		NUMERIC, 
	@EntryDateTime	DATETIME
	
 AS
	
BEGIN
	
	--IT IS FOR THE INSERT
	
	SELECT CustomerId FROM CustomerFavourites WHERE CustomerId = @CustomerId AND VersionId = @VersionId

	IF @@ROWCOUNT = 0
		INSERT INTO CustomerFavourites
			(
				CustomerId, 		VersionId, 		EntryDateTime
			)
			VALUES
			(	
				@CustomerId, 		@VersionId, 		@EntryDateTime
			)

		
END
