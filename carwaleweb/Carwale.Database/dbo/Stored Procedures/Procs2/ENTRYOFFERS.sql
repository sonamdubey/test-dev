IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ENTRYOFFERS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ENTRYOFFERS]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR offers and reviews
--ID, DealerId, CategoryId, Title, EntryDate, ExpiryDate

CREATE PROCEDURE [dbo].[ENTRYOFFERS]
	@ID					NUMERIC,			--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@DealerId			NUMERIC,			--ID OF THE DealerId
	@CategoryId			NUMERIC,			--ID OF THE category
	@Priority			SMALLINT,			--@Priority
	@Title				VARCHAR(200),		--TITLE
	@EntryDate 			DATETIME,			--DATE OF THE entry
	@ExpiryDate 		DATETIME,			--DATE OF THE expiry
	@RebateDescription	VARCHAR(MAX) = NULL,	--Description
	@OfferId			NUMERIC OUTPUT
 AS
	
BEGIN
	
	SET NOCOUNT ON;
	
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		INSERT INTO OffersRebates
			(
				DealerId,  CategoryId, 	Title, 	EntryDate, 	ExpiryDate,	 Priority,  RebateDescription
			) 
		VALUES
			(
				@DealerId, @CategoryId, @Title, @EntryDate, @ExpiryDate, @Priority, @RebateDescription
			)

		SET @OfferId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE OffersRebates SET 
			DealerId			= @DealerId,
			CategoryId 			= @CategoryId,
			Title	 			= @Title,
			EntryDate			= @EntryDate,
			ExpiryDate 			= @ExpiryDate,
			Priority			= @Priority,	
			RebateDescription	= @RebateDescription
		 WHERE 
			ID = @ID

		SET @OfferId = @ID
	END
	
		
END

