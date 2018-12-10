IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FetchStockReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FetchStockReport]
GO
	
-- Modifier : Sachin Bharti(3rd of May 2013)
-- Purpose	: Remove roleId constraint on users because now 
--			  we get user based on their roles
CREATE PROCEDURE [dbo].[DCRM_FetchStockReport]
	
		@RegionId NUMERIC = NULL,
		@CityId NUMERIC = NULL,
		@ExecId NUMERIC = NULL
AS
BEGIN

	SELECT DISTINCT SI.ID, CMa.Name Make, CMo.Name Model, CV.Name Version, SI.Kilometers, 
	REPLACE(RIGHT(CONVERT(VARCHAR(11),SI.MakeYear, 106), 8), ' ', '-') AS MakeYear, 
	SI.Price, DATEDIFF(d, SI.EntryDate, GETDATE()) AS Age,
	CONVERT(VARCHAR,SI.EntryDate,106) As EntryDate,
 
	CASE WHEN SI.ViewCount IS NUll THEN 0 ELSE SI.ViewCount END ViewCount, 
	CASE WHEN BDC.Valuation IS NULL THEN 0 ELSE BDC.Valuation END CWPrice, 
	
	(SELECT COUNT(InquiryId)FROM CarPhotos WHERE isActive=1 AND InquiryId = SI.ID )  
	AS CarWithoutPhoto,

	(SELECT COUNT(SellInquiryId) AS CntSells FROM UsedCarPurchaseInquiries
	WHERE SellInquiryId =SI.ID) Response,
	
	SI.Price - CASE WHEN BDC.Valuation IS NULL THEN 0 ELSE BDC.Valuation END OverPrice,
	SI.Comments, DFC.DFC_Id, DCO.ORKm, 
	CAST(((CASE WHEN BDC.Valuation IS NULL THEN 0 ELSE BDC.Valuation END)*20)/ 100.0 as numeric(18,0))
	PerVal, SI.Color, SI.CarRegNo, D.Organization, R.Name AS Region, C.Name, D.Organization, SI.DealerId
 
 	FROM
	SellInquiries SI INNER JOIN  CarVersions CV ON SI.CarVersionId = CV.ID
	INNER JOIN Dealers D ON SI.DealerId = D.ID
	INNER JOIN DCRM_ADM_RegionCities DAR ON D.CityId = DAR.CityId
	INNER JOIN DCRM_ADM_Regions R ON DAR.RegionId = R.Id
	INNER JOIN Cities C ON D.CityId = C.ID
	INNER JOIN CarModels CMo ON CV.CarModelId = CMo.ID 
	INNER JOIN CarMakes CMa ON CMo.CarMakeId = CMa.ID 
	LEFT JOIN BestDealCarValuations BDC ON SI.ID = BDC.CarId
	LEFT JOIN DealerFeaturedCars DFC ON SI.Id = DFC.CarId
	LEFT JOIN DCRM_CarsORData DCO ON SI.ID = DCO.InquiryId AND DCO.ORKm = 1
	INNER JOIN DCRM_ADM_UserDealers DAU ON D.ID = DAU.DealerId --AND DAU.RoleId = 4

	
	WHERE  SI.StatusId=1  
	AND Convert(Date, SI.PackageExpiryDate) >= Convert(Date, GETDATE())
	AND R.Id = COALESCE(@RegionId, R.Id) AND C.ID = COALESCE(@CityId, C.ID)
	AND DAU.UserId = COALESCE(@ExecId , DAU.UserId)
	ORDER BY CMa.Name
END

