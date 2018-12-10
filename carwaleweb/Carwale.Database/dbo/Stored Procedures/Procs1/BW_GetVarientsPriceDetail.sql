IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetVarientsPriceDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetVarientsPriceDetail]
GO

	-- Author:		Sumit Kate
-- Create date: 14 Sept 2015
-- Description:	To get the new bike booking page data which includes
--				bike details,price split of all the varients, dealer disclaimer
-- Modified	by	: Sumit Kate
--	Date		: 24 Sept 2015
-- Descrption	: replaced the INNER JOIN with LEFT JOIN in second result set
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetVarientsPriceDetail]
@CityId INT,
@VersionId INT,
@DealerId INT
AS
BEGIN

DECLARE @ModelId INT
--SET @VersionId = 165
--SET @CityId = 1

SELECT @ModelId = BV.BikeModelId
	FROM BikeVersions BV WITH (NOLOCK)
	WHERE BV.IsDeleted = 0
		AND BV.New = 1
		AND BV.ID = @VersionId

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
			AND DSP.BikeVersionId IN (SELECT ID FROM BikeVersions WHERE BikeModelId = @ModelId)
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
		,ISNULL(BA.Amount,0) AS BookingAmount			
		,ISNULL(BAV.NumOfDays,0) AS NumOfDays
		,SPEC.AlloyWheels AS AlloyWheels
		,SPEC.BrakeType AS BreakType
		,SPEC.ElectricStart AS ElectricStart
		,ISNULL(SPEC.AntilockBrakingSystem,0) AS [ABS]
	FROM BikeVersions BV WITH (NOLOCK)
	INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes BM WITH (NOLOCK) ON BM.ID = BMO.BikeMakeId	
	INNER JOIN CTE ON CTE.VersionId = BV.ID
	LEFT JOIN NewBikeSpecifications SPEC WITH(NOLOCK) ON SPEC.BikeVersionId = BV.Id
	LEFT JOIN BW_DealerBikeBookingAmounts BA WITH(NOLOCK) ON BA.DealerId = @DealerId AND BA.VersionId = BV.ID AND BA.IsActive = 1
	LEFT JOIN BW_BikeAvailability BAV WITH (NOLOCK) ON CTE.VersionId = BAV.BikeVersionId AND BAV.DealerId = @DealerId AND BAV.IsActive = 1
	WHERE OnRoadPrice > 0
		AND BV.New = 1
		AND BMO.New = 1
-- Query to get the dealer disclaimer
SELECT Disclaimer
FROM BW_DealerDisclaimer dd WITH(NOLOCK)
INNER JOIN BikeVersions BV WITH(NOLOCK) ON dd.BikeVersionId = BV.ID
INNER JOIN BikeModels BMO WITH(NOLOCK) ON BMO.ID = @ModelId
WHERE DealerId = @DealerId
	AND BikeVersionId = @VersionId
	AND IsActive = 1

-- SELECT CLAUSE TO GET OFFER LIST OFFERED BY DEALER FOR GIVEN MODEL
SELECT PQO.OfferCategoryId
	,PQO.OfferText
	,ISNULL(PQO.OfferValue, 0) OfferValue	
	,PQOC.OfferType
FROM BW_PQ_Offers AS PQO WITH (NOLOCK)
INNER JOIN BW_PQ_OfferCategories AS PQOC WITH (NOLOCK) ON PQO.OfferCategoryId = PQOC.Id
INNER JOIN BikeModels AS BMO WITH (NOLOCK) ON BMO.ID = PQO.ModelId
	AND BMO.IsDeleted = 0
	AND BMO.New = 1
WHERE PQO.IsActive = 1
	AND PQOC.IsActive = 1
	AND PQO.DealerId = @DealerId
	AND PQO.CityId = @CityId
	AND PQO.OfferValidTill > GETDATE()
	AND PQO.ModelId = @ModelId
ORDER BY PQOC.Id

SELECT 
	dealer.ID,
	FirstName,
	LastName,
	EmailId,
	Organization,
	Address1,
	Address2,
	dealer.Pincode,
	PhoneNo,
	FaxNo,
	MobileNo,
	WebsiteUrl,
	ContactHours,
	state.Name AS StateName,
	city.Name AS CityName,
	area.Name AS AreaName,
	dealer.Lattitude,
	dealer.Longitude
FROM Dealers dealer WITH(NOLOCK)
INNER JOIN States [state] ON dealer.StateId = state.ID
INNER JOIN Cities [city] ON dealer.CityId = city.ID
INNER JOIN Areas [area] ON dealer.AreaId = area.ID
WHERE dealer.ID = @DealerId
END
