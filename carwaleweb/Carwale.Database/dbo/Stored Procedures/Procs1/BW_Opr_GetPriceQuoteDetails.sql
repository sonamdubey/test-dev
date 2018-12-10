IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_Opr_GetPriceQuoteDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_Opr_GetPriceQuoteDetails]
GO

	
-------------------------------------------------------------
-- Created By : Sadhana Upadhyay on 23 Oct 2015
-- Summary : To get DealerPrice quote Details
-- Modified  By : Sushil Kumar
-- Summary  : Added provision to fetch color available with dealer/s
-- EXEC BW_Opr_GetPriceQuoteDetails '10671,10273,4,10278', 164, 1
-- Modified By : Sadhana Upadhyay on 11 Jan 2016
-- Summary : To get Bike availability by color
-- Modified By : Sadhana Upadhyay on 13 Jan 2016
-- Summary : To get color availability
-------------------------------------------------------------
CREATE PROCEDURE [dbo].[BW_Opr_GetPriceQuoteDetails] @DealerIds VARCHAR(MAX)
	,@BikeVersionId INT
	,@CityId INT
AS
BEGIN
	DECLARE @ModelId INT

	SELECT @ModelId = BikeModelId
	FROM BikeVersions WITH (NOLOCK)
	WHERE ID = @BikeVersionId

	SELECT D.FirstName + ' ' + D.LastName AS NAME
		,D.Organization
		,D.Address1 + ' ' + D.Address2 AS [Address]
		,D.ContactHours
		,D.MobileNo
		,D.ID AS DealerId
		,ISNULL(BA.NumOfDays, 0) AS NumOfDays
		,ISNULL(BAA.Amount, 0) AS BookingAmount
	FROM Dealers AS D WITH (NOLOCK)
	INNER JOIN dbo.fnSplitCSVValues(@DealerIds) FN ON D.ID = FN.ListMember
	LEFT JOIN BW_BikeAvailability BA WITH (NOLOCK) ON D.ID = BA.DealerId
		AND BA.IsActive = 1
		AND BA.BikeVersionId = @BikeVersionId
	LEFT JOIN BW_DealerBikeBookingAmounts BAA WITH (NOLOCK) ON D.ID = BAA.DealerId
		AND BAA.IsActive = 1
		AND BAA.VersionId = @BikeVersionId

	SELECT BWO.DealerId
		,BWO.OfferText
		,BWO.OfferValue
		,BWO.OfferValidTill
		,OC.OfferType
		,BWO.Id AS OfferId
		,oc.Id AS OfferCateGoryId
	FROM BW_PQ_Offers BWO WITH (NOLOCK)
	INNER JOIN dbo.fnSplitCSVValues(@DealerIds) FN ON BWO.DealerId = FN.ListMember
	INNER JOIN BW_PQ_OfferCategories OC WITH (NOLOCK) ON OC.Id = BWO.OfferCategoryId
	WHERE CityId = @CityId
		AND ModelId = @ModelId
		AND BWO.IsActive = 1

	SELECT CI.ItemName
		,DSP.ItemValue AS Price
		,DSP.DealerId
		,DSP.ItemId
	FROM BW_NewBikeDealerShowroomPrices DSP WITH (NOLOCK)
	INNER JOIN BW_PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = DSP.ItemId
	INNER JOIN dbo.fnSplitCSVValues(@DealerIds) FN ON DSP.DealerId = FN.ListMember
	WHERE DSP.CityId = @CityId
		AND DSP.BikeVersionId = @BikeVersionId
		AND DSP.ItemValue > 0
	ORDER BY DSP.ItemId

	-- Added By : Sadhana Upadhyay on 11 Jan 2016
	;WITH CTE AS (
		SELECT DISTINCT  BVC.ModelColorID AS ColorId
			,BMC.ColorName
			,BCC.HexCode
			,ISNULL(BVC.VersionId, 0) AS BikeVersionId
			,D.Id AS DealerId
			,COALESCE(BAC.NumOfDays, BA.NumOfDays, - 1) AS NumOfDays
		FROM BikeVersionColorsMaster BVC WITH (NOLOCK)
		INNER JOIN BikeModelColors BMC WITH (NOLOCK) ON BMC.ID = BVC.ModelColorID
			AND bmc.IsActive = 1
		INNER JOIN BikeColorCodes BCC WITH (NOLOCK) ON BCC.ModelColorID = BMC.ID
			AND BCC.IsActive = 1
		INNER JOIN Dealers AS D WITH (NOLOCK)  ON 1=1
		INNER JOIN dbo.fnSplitCSVValues(@DealerIds) FN ON D.Id = FN.ListMember
		LEFT JOIN BW_BikeAvailabilityByColor BAC WITH (NOLOCK) ON BAC.ColorId = BMC.ID
			                                                 AND BVC.VersionId = BAC.BikeVersionId 
			                                                 AND D.ID=BAC.DealerId
	     LEFT JOIN BW_BikeAvailability BA WITH (NOLOCK) ON BVC.VersionId = BA.BikeVersionId
			                                           AND BA.IsActive = 1
			                                           AND D.ID=BA.DealerId
		WHERE BVC.VersionId = @BikeVersionId
			AND BVC.IsActive = 1
		) 
	SELECT ColorId,ColorName,HexCode,BikeVersionId,DealerId,NumOfDays FROM CTE WITH (NOLOCK) ORDER BY Dealerid
		,BikeVersionId
END
