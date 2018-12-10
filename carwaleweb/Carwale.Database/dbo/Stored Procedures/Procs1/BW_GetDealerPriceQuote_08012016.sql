IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerPriceQuote_08012016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerPriceQuote_08012016]
GO

	
---------------------------------------------------------
-- Created By : Sadhana Upadhyay on 22nd Oct 2014
-- Summary : To get dealer price quote
--EXEC BW_GetDealerPriceQuote 1,14,22
-- Modified By : Sadhana Upadhyay on 8 Dec 2014
-- Summary : To retrieve Disclaimer
-- Modified By : Ashish G. Kamble on 24 June 2015
-- Modified : BW_GetDealerPriceQuote sp modified to get the offers data.
-- Modified By : Sumit Kate
-- Summary	: Added OriginalImagePath column to output
-- Modified By	:	Sangram Nandkhile on 05 Oct 2015 : Added Join and column to check if offer has view terms or not
-- Modified By	:	Sumit Kate on 08 Oct 2015
-- Summary		:	Return the On Road Price For Other varients
-- Modified By	:	Sushil Kumar on 11 Nov 2015
-- Summary		:	Return the Booking amount for each varient
-- Modified By	:	Sangram Nandkhile on 08 Jan 2016 :-> IsPriceImpact columns is added in selection
---------------------------------------------------------
CREATE PROCEDURE [dbo].[BW_GetDealerPriceQuote_08012016] 
	@CityId INT
	,@VersionId INT
	,@DealerId INT = NULL
AS
BEGIN
	-- Query to get version details
	SELECT BM.NAME AS MakeName
		,BMO.NAME AS ModelName
		,BV.NAME AS VersionName
		,BM.MaskingName AS MakeMaskingName
		,BMO.MaskingName AS ModelMaskingName
		,BV.HostURL
		,BV.largePic
		,BV.smallPic
		,BV.OriginalImagePath
	FROM BikeVersions BV WITH (NOLOCK)
	INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes BM WITH (NOLOCK) ON BM.ID = BMO.BikeMakeId
	WHERE BV.ID = @VersionId
		AND BV.IsDeleted = 0
		AND BMO.IsDeleted = 0
		AND BM.IsDeleted = 0
		AND BV.Futuristic = 0
		AND BMO.Futuristic = 0
		AND BM.Futuristic = 0

	-- Query to get the pricing
	SELECT CI.ItemName
		,DSP.ItemValue AS Price
		,DSP.DealerId
		,DSP.ItemId
	FROM BW_NewBikeDealerShowroomPrices DSP WITH (NOLOCK)
	INNER JOIN BW_PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = DSP.ItemId
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DSP.DealerId
	WHERE DSP.CityId = @CityId
		AND DSP.BikeVersionId = @VersionId
		AND D.IsDealerDeleted = 0
		AND D.IsDealerActive = 1
		AND D.ApplicationId = 2
		AND DSP.ItemValue > 0
		AND (
			@DealerId IS NULL
			OR DSP.DealerId = @DealerId
			)
	ORDER BY DSP.ItemId

	-- Query to get the dealer disclaimer
	SELECT Disclaimer
	FROM BW_DealerDisclaimer WITH (NOLOCK)
	WHERE DealerId = @DealerId
		AND BikeVersionId = @VersionId
		AND IsActive = 1

	DECLARE @ModelId INT

	SELECT @ModelId = BV.BikeModelId
	FROM BikeVersions BV WITH (NOLOCK)
	WHERE BV.IsDeleted = 0
		AND BV.New = 1
		AND BV.ID = @VersionId

	-- SELECT CLAUSE TO GET OFFER LIST OFFERED BY DEALER FOR GIVEN MODEL
	SELECT PQO.OfferCategoryId
		,PQO.OfferText
		,ISNULL(PQO.OfferValue, 0) OfferValue
		,PQO.EntryDate
		,PQOC.OfferType
		,PQO.OfferId -- Added by Sangram on 5 oct 2015
		,Case when BO.id is NULL then 'False' else 'True'end as 'IsOfferTerms'-- Added by Sangram on 5 oct 2015
		,PQO.IsPriceImpact
	FROM BW_PQ_Offers AS PQO WITH (NOLOCK)
	INNER JOIN BW_PQ_OfferCategories AS PQOC WITH (NOLOCK) ON PQO.OfferCategoryId = PQOC.Id
	INNER JOIN BikeModels AS BMO WITH (NOLOCK) ON BMO.ID = PQO.ModelId
	LEFT outer Join BW_offers as BO WITH (NOLOCK) ON  PQO.offerid = BO.id -- Added by Sangram on 5 oct 2015
		AND BMO.IsDeleted = 0
		AND BMO.New = 1
	WHERE PQO.IsActive = 1
		AND PQOC.IsActive = 1
		AND PQO.DealerId = @DealerId
		AND PQO.CityId = @CityId
		AND PQO.OfferValidTill > GETDATE()
		AND PQO.ModelId = @ModelId
	ORDER BY PQOC.Id

	--On road price for the varients
	
	;WITH CTE(OnRoadPrice,VersionId) AS
	(
	SELECT
	SUM(DSP.ItemValue) AS OnRoadPrice
	,DSP.BikeVersionId AS VersionId	
	FROM BW_NewBikeDealerShowroomPrices DSP WITH (NOLOCK)
	INNER JOIN BW_PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = DSP.ItemId
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DSP.DealerId	
	LEFT JOIN BW_BikeAvailability BAV WITH (NOLOCK) ON BAV.BikeVersionId = DSP.BikeVersionId AND D.ID = BAV.DealerId
	WHERE DSP.CityId = @CityId
	AND DSP.BikeVersionId IN (SELECT ID FROM BikeVersions WITH (NOLOCK) WHERE BikeModelId = @ModelId)
	AND D.[Status] = 0
	AND D.ApplicationId = 2
	AND DSP.ItemValue > 0
	AND DSP.DealerId = @DealerId

	GROUP BY DSP.BikeVersionId)
	SELECT
	BV.ID AS [VersionId]
	,BM.ID AS MakeId
	,BMO.ID AS ModelId
	,BM.NAME AS MakeName
	,BMO.NAME AS ModelName
	,BV.NAME AS VersionName
	,BM.MaskingName AS MakeMaskingName
	,BMO.MaskingName AS ModelMaskingName
	,BMO.HostURL
	,BMO.OriginalImagePath
	,CTE.OnRoadPrice
	,ISNULL(BA.Amount,0) BookingAmount
	FROM BikeVersions BV WITH (NOLOCK)
	INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes BM WITH (NOLOCK) ON BM.ID = BMO.BikeMakeId	
	INNER JOIN CTE WITH (NOLOCK) ON CTE.VersionId = BV.ID	
	LEFT JOIN BW_DealerBikeBookingAmounts BA WITH(NOLOCK) ON BV.ID = BA.VersionId	
	AND BA.DealerId = @DealerId
	AND BA.IsActive = 1
	WHERE OnRoadPrice > 0
	AND BV.New = 1
	AND BMO.New = 1

	--Breakup of on road price for varients
	SELECT CI.ItemName
		,DSP.ItemValue AS Price
		,DSP.DealerId
		,DSP.ItemId
		,DSP.BikeVersionId AS [VersionId]		
	FROM BW_NewBikeDealerShowroomPrices DSP WITH (NOLOCK)
	INNER JOIN BW_PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = DSP.ItemId
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DSP.DealerId
	INNER JOIN BikeVersions BV WITH(NOLOCK) ON DSP.BikeVersionId = BV.ID
	INNER JOIN BikeModels BMO WITH(NOLOCK) ON BMO.ID = BV.BikeModelId
	WHERE DSP.CityId = @CityId	
		AND BMO.ID = @ModelId
		AND D.[Status] = 0
		AND D.ApplicationId = 2
		AND DSP.ItemValue > 0
		AND DSP.DealerId = @DealerId
		AND BV.New = 1
		AND BMO.New = 1	
ORDER BY DSP.ItemId
END
