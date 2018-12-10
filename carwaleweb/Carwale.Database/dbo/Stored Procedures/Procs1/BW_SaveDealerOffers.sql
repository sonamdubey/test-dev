IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveDealerOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveDealerOffers]
GO

	
-- =============================================
-- Modified By : Sadhana Upadhyay on 8 Oct 2015
-- Summary : To save multiple offers
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveDealerOffers] 
	@DealerId INT
	,@CityId INT
	,@ModelId VARCHAR(MAX)
	,@OffercategoryId INT
	,@OfferText VARCHAR(MAX)
	,@OfferValue INT
	,@IsActive BIT = 1
	,@OffervalidTill DATETIME
	,@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TempOfferList TABLE (
		DealerId INT
		,CityId INT
		,ModelId INT
		,OffercategoryId INT
		,OfferText VARCHAR(MAX)
		,OfferValue INT
		,OffervalidTill DATETIME
		,UserId INT
		,IsActive BIT
		)

	INSERT INTO @TempOfferList (
		DealerId
		,CityId
		,ModelId
		,OffercategoryId
		,OfferText
		,OfferValue
		,OffervalidTill
		,UserId
		,IsActive
		)
	SELECT @DealerId
		,@CityId
		,FN.ListMember
		,@OffercategoryId
		,@OfferText
		,@OfferValue
		,@OffervalidTill
		,@UserId
		,@IsActive
	FROM dbo.fnSplitCSVValues(@ModelId) FN;

	MERGE BW_PQ_Offers AS BWO
	USING (
		SELECT *
		FROM @TempOfferList
		) AS TEMP
		ON TEMP.DealerId = BWO.DealerId
			AND TEMP.CityId = BWO.CityId
			AND TEMP.ModelId = BWO.ModelId
			AND TEMP.OfferCategoryId = BWO.OfferCategoryId
			AND TEMP.OfferText = BWO.OfferText
			AND TEMP.OfferValue = BWO.OfferValue
			AND TEMP.OffervalidTill = BWO.OffervalidTill
			AND TEMP.IsActive=BWO.IsActive
	WHEN NOT MATCHED
		THEN
			INSERT (
				DealerId
				,CityId
				,ModelId
				,OfferCategoryId
				,OfferText
				,OfferValue
				,EntryDate
				,LastUpdated
				,OfferValidTill
				,UpdatedBy
				,IsActive
				)
			VALUES (
				TEMP.DealerId
				,TEMP.CityId
				,TEMP.ModelId
				,TEMP.OffercategoryId
				,TEMP.OfferText
				,TEMP.OfferValue
				,GETDATE()
				,GETDATE()
				,TEMP.OffervalidTill
				,@UserId
				,1
				);
END
