IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarPrices]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 7 Feb 2014
-- Description:	To get the New Car Prices
-- =============================================
CREATE PROCEDURE [dbo].[GetNewCarPrices]
	@ModelId INT,
	@VersionId INT,
	@CityId INT
AS
BEGIN
	DECLARE @RegionCityId VARCHAR(20)
	IF @CityId = 12
		SET @RegionCityId = '12,646,647'
	ELSE IF @CityId = 40
		SET @RegionCityId = '40,645'
	ELSE
		SET @RegionCityId = @CityId

	DECLARE @SelectedVersionId INT = NULL
	IF(@VersionId != 0)
		SET	@SelectedVersionId = @VersionId
	
	SELECT (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CV.Id AS VersionId,C.Name AS CityName,
	ISNULL(NCSP.Price,0) AS CPrice, ISNULL(NCSP.RTO,0) AS CRTO, ISNULL(NCSP.Insurance,0) AS CInsurance,
	 NCSP.LastUpdated, 
	ISNULL((SELECT MIN(Discount) FROM NCS_IndependentDeal AS NID, NCS_Dealers AS ND WHERE ND.Id = NID.DealerId AND NID.VersionId = CV.Id AND ND.CityId IN(SELECT LISTMEMBER FROM fnSplitCSV(@RegionCityId))),0) AS CDiscount,
	(NCSP.Price+NCSP.RTO+NCSP.Insurance-ISNULL((SELECT MIN(Discount) FROM NCS_IndependentDeal AS NID, NCS_Dealers AS ND WHERE ND.Id = NID.DealerId AND NID.VersionId = CV.Id AND ND.CityId = 1),0)) AS OnRoadPrice 
	FROM	CarMakes AS CMA 
			LEFT JOIN CarModels AS CMO ON CMA.Id = CMO.CarMakeId 
			LEFT JOIN CarVersions AS CV ON CMO.Id = CV.CarModelId AND CV.IsDeleted =0
			LEFT JOIN NewCarShowroomPrices AS NCSP ON CV.Id = NCSP.CarVersionId	
			LEFT JOIN Cities AS C ON C.Id = NCSP.CityId
	WHERE	(@SelectedVersionId IS NULL AND CMO.Id = @ModelId AND C.Id IN(SELECT LISTMEMBER FROM fnSplitCSV(@RegionCityId)))
			OR
			(@SelectedVersionId IS NOT NULL AND CV.Id = @VersionId AND C.Id IN(SELECT LISTMEMBER FROM fnSplitCSV(@RegionCityId)))
			AND NCSP.IsActive = 1
	ORDER BY OnRoadPrice ASC

	SELECT NP.CarVersionId AS VersionId, CI.CategoryName, CI.Id as ItemId, NP.PQ_CategoryItemValue AS ItemValue,
	CI.CategoryId
	FROM CW_NewCarShowroomPrices NP WITH(NOLOCK)
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = NP.CarVersionId AND CV.CarModelId = @ModelId AND CV.IsDeleted =0
	INNER JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = NP.PQ_CategoryItem			
	WHERE NP.PQ_CategoryItem NOT IN (2,3,5) and NP.CityId IN(SELECT LISTMEMBER FROM fnSplitCSV(@RegionCityId))
END

