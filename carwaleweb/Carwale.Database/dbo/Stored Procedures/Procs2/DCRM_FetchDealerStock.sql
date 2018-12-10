IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FetchDealerStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FetchDealerStock]
GO

	
--==============================================
 -- Updated By : Ajay Singh 8th Feb 2016 
 -- Purpose   :  Add viewcount and Impression  by joinning table "DealerUsedCarViews".
--=============================================

CREATE PROCEDURE [dbo].[DCRM_FetchDealerStock] 
	
		@DealerId NUMERIC
AS
BEGIN

	SELECT DISTINCT SI.ID, CMa.Name Make, CMo.Name Model, CV.Name Version, SI.Kilometers, SI.DealerId,
		REPLACE(RIGHT(CONVERT(VARCHAR(11),SI.MakeYear, 106), 8), ' ', '-') AS MakeYear, 
		YEAR(SI.MakeYear) AS MkYear,
		MONTH(SI.MakeYear)AS MKMonth,
		D.CityId ,CV.ID AS VersionId,
		SI.Price, DATEDIFF(d, SI.EntryDate, GETDATE()) AS Age,
		CONVERT(VARCHAR,SI.EntryDate,106) As EntryDate,
		--Added Lastupdated for Stock Updated before 6 month 
		DATEDIFF(d,SI.LastUpdated,GETDATE()) AS LastUpdated,
 
		ISNULL(DUC.Viewcount,0) AS ViewCountNew,
		CASE WHEN SI.ViewCount IS NUll THEN 0 ELSE SI.ViewCount END ViewCount, 
		CASE WHEN BDC.Valuation IS NULL THEN 0 ELSE BDC.Valuation END CWPrice, 
		BDC.Id AS CarValuationId,

		--(SELECT CASE WHEN COUNT(InquiryId) > 0 THEN 'FALSE' ELSE 'TRUE' END 
		--FROM CarPhotos WHERE isActive=1 AND InquiryId = SI.ID )  
		--AS CarWithoutPhoto,
	
		(SELECT COUNT(InquiryId)FROM CarPhotos WITH (NOLOCK) WHERE isActive=1 AND InquiryId = SI.ID AND Isdealer = 1)  
		  AS CarWithoutPhoto,

		(SELECT COUNT(SellInquiryId) AS CntSells FROM UsedCarPurchaseInquiries WITH (NOLOCK)
		   WHERE SellInquiryId =SI.ID) Response,
	
		--SI.Price - CASE WHEN BDC.Valuation IS NULL THEN 0 ELSE BDC.Valuation END OverPrice,
		SI.Price - CASE WHEN BDC.Valuation IS NULL THEN 0 ELSE BDC.Valuation END OverPrice,
		SI.Comments, DFC.DFC_Id, DCO.ORKm, 
		CAST(((CASE WHEN BDC.Valuation IS NULL THEN 0 ELSE BDC.Valuation END)*20)/ 100.0 as numeric(18,0))
		PerVal, SI.Color, SI.CarRegNo,
		ISNULL(DUC.Impression,-1) AS Impression
 
	FROM
		SellInquiries SI WITH (NOLOCK) 
		INNER JOIN	Dealers D(NOLOCK) ON D.ID = SI.DealerId
		INNER JOIN  CarVersions CV WITH (NOLOCK) ON SI.CarVersionId = CV.ID
		INNER JOIN	CarModels CMo WITH (NOLOCK) ON CV.CarModelId = CMo.ID 
		INNER JOIN	CarMakes CMa WITH (NOLOCK) ON CMo.CarMakeId = CMa.ID 
		LEFT  JOIN	BestDealCarValuations BDC WITH (NOLOCK) ON SI.ID = BDC.CarId
		LEFT  JOIN	DealerFeaturedCars DFC WITH (NOLOCK) ON SI.Id = DFC.CarId
		LEFT  JOIN	DCRM_CarsORData DCO WITH (NOLOCK) ON SI.ID = DCO.InquiryId AND DCO.ORKm = 1
		LEFT  JOIN  DealerUsedCarViews DUC WITH(NOLOCK) ON SI.ID = DUC.InquiryID
	
	WHERE 
		SI.DealerId = @DealerId 
		AND	SI.StatusId=1  
		AND Convert(Date, SI.PackageExpiryDate) >= Convert(Date, GETDATE())
	ORDER BY CMA.Name
END




