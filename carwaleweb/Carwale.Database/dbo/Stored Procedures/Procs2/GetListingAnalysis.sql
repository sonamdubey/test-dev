IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetListingAnalysis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetListingAnalysis]
GO

	CREATE PROCEDURE [dbo].[GetListingAnalysis] 
	@CarVersionId		NUMERIC,
	@CurrentDate		DATETIME,
	@LastDate		DATETIME,
	@Listings		NUMERIC OUTPUT,
	@ProfileViews		NUMERIC OUTPUT,
	@PurchaseInquiries	NUMERIC OUTPUT,
	@Valuations		NUMERIC OUTPUT

AS
	DECLARE 	@TempId		NUMERIC,
			@CarModelId		NUMERIC
BEGIN
	
	SELECT @Listings=Listings, @ProfileViews=ProfileViews, @PurchaseInquiries=PurchaseInquiries, @Valuations=Valuations
	FROM ListingAnalysis
	WHERE CarModelId IN (Select CarModelId From CarVersions Where ID = @CarVersionId) 

END