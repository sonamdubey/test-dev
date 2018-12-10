IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_SaveListingAnalysis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_SaveListingAnalysis]
GO

	CREATE PROCEDURE [dbo].[AP_SaveListingAnalysis]
	@CarModelId			NUMERIC,
	@Listings			NUMERIC, 
	@ProfileViews		NUMERIC,
	@PurchaseInquiries	NUMERIC,
	@Valuations			NUMERIC,
	@LastUpdated		DATETIME,
	@Status				BIT OUTPUT	
 AS
	
BEGIN
	SET @Status = 0

		UPDATE ListingAnalysis 
		SET Listings =  @Listings, ProfileViews = @ProfileViews, 
			PurchaseInquiries = @PurchaseInquiries, Valuations = @Valuations,
			LastUpdated = @LastUpdated
		WHERE CarModelId = @CarModelId
				
		IF @@RowCount = 0
			BEGIN
				INSERT INTO ListingAnalysis
						(CarModelId, Listings, ProfileViews, PurchaseInquiries, Valuations, LastUpdated) 
				VALUES	(@CarModelId, @Listings, @ProfileViews, @PurchaseInquiries, @Valuations, @LastUpdated)
				
				SET @Status = 1 
			END
			
		ELSE

			SET @Status = 1 
			
END



