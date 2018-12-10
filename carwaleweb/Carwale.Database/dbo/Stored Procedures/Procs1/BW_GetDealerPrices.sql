IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerPrices]
GO

	-- =============================================
-- Author:		Aswini Todkar 
-- Create date: 5 Nov 2014
-- Description:	Proc to Get all bike version and prices 

-- Modified By : Suresh Prajapati on 20th Jan, 2015
-- Description : To get prices with make-model specified
-- BW_GetDealerPrices 15,1,4

-- Modified By : Suresh Prajapati on 22nd Jan, 2015
-- Description : Added Bike Availability Information
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerPrices]
	-- Add the parameters for the stored procedure here
	@MakeId INT
	--,@ModelId INT
	,@CityId INT
	,@DealerId INT
AS
BEGIN
	SELECT BMA.NAME AS Make
		,BMO.NAME AS Model
		,BV.NAME AS VersionName
		,BV.ID AS VersionId
		,CI.ItemName
		,SP.ItemValue
		,BA.NumOfDays
		--,BA.IsActive
	--,DATEDIFF(dd, SP.EntryDate, GETDATE()) UpdatedBeforeDays
	FROM BikeVersions BV WITH (NOLOCK) 
	LEFT JOIN BW_NewBikeDealerShowroomPrices SP WITH (NOLOCK) ON SP.BikeVersionId = BV.ID
		AND SP.CityId = @CityId
		AND SP.DealerId = @DealerId
		AND SP.ItemId = 1
	LEFT JOIN BW_PQ_CategoryItems AS CI WITH (NOLOCK) ON CI.ItemCategoryId = SP.ItemId
	LEFT JOIN BW_BikeAvailability AS BA WITH (NOLOCK) ON BA.BikeVersionId = BV.ID AND BA.IsActive=1
	AND BA.DealerId = @DealerId
	INNER JOIN BikeModels AS BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes AS BMA WITH (NOLOCK) ON BMA.ID = BMO.BikeMakeId
	WHERE BV.IsDeleted = 0
		AND BV.New = 1
		AND BMO.IsDeleted = 0
		AND BMO.New = 1
		AND BMA.IsDeleted = 0
		AND BMA.New = 1
		AND BMA.ID = @MakeId
	ORDER BY BMA.Name, BMO.Name, BV.Name

	SELECT BV.ID VersionId
		,SP.ItemValue
		,CI.ItemName
		,CI.ItemCategoryId
	FROM BW_NewBikeDealerShowroomPrices SP WITH (NOLOCK)
	INNER JOIN BikeVersions BV WITH (NOLOCK) ON SP.BikeVersionId = BV.ID
	INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BW_PQ_CategoryItems CI WITH (NOLOCK) ON CI.ItemCategoryId = SP.ItemId
	WHERE SP.CityId = @CityId
		AND BV.New = 1
		AND SP.DealerId = @DealerId
		AND BMO.BikeMakeId = @MakeId
		AND BV.IsDeleted = 0
	ORDER BY CI.ItemCategoryId ASC
END
