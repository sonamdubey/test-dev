IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDealers_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDealers_V15]
GO

	-- =============================================          
-- Author:  <supriya > 
-- Created On  : 10/10/2014          
-- Description : Reference of the sp "GetNCDDealers" and made changes to get DealerContent,ShowroomImage,StartTime and EndTime
-- Description : Reference of the sp [dbo].[GetNCDDealers_V1.0]
-- Created by Supriya Khartode on 10/10/2014 to fetch mobileno from PrimaryMobileNo column 
-- Modified by Supriya Khartode on 4/11/2014 to fetch makename & landlineno column
-- Modified By sanjay soni: 23/2/15 removed dependency of ncd_dealer  
-- Modified By Shalini Nair on 12/06/15 added Subcontentcatagory is null check to retrieve distinct records
--Modified By Shalini Nair on 12/08/15 retrieving originalImgPath 
-- Modified by Manish on 21 Oct 2015 added partitioned by for removing duplicate records
-- =============================================          
CREATE  PROCEDURE [dbo].[GetNCDDealers_V15.10.2_Test] (
	@CityId SMALLINT
	,@MakeId SMALLINT
	)
AS
BEGIN
	SET NOCOUNT ON;

	WITH cte
	AS (
		SELECT DISTINCT DNC.TcDealerId AS Id
			,DNC.Id NewCarDealerId
			,DNC.NAME AS DealerName
			,DNC.Address
			,DNC.PinCode
			,DNC.FaxNo
			,DNC.EMailId
			,DNC.WebSite AS WebSite
			,DNC.WorkingHours
			,'+91' + DNC.Mobile AS MobileNo -- modified by sanjay on 27/2/2015
			,CMA.NAME AS CarMake
			,C.NAME AS City
			,S.NAME AS STATE
			,DNC.Latitude AS Latitude -- modeified by sanjay 23/2/15
			,DNC.Longitude AS Longitude -- modeified by sanjay 23/2/15
			,1 AS IsPremium
			,mdc.DealerContent
			,DNC.ShowroomStartTime AS StartTime
			,DNC.ShowroomEndTime AS EndTime
			,mi.HostURL + mi.DirectoryPath + mi.ImgPathCustom600 AS ShowroomImage
			,CMA.NAME AS MakeName
			,DNC.LandLineNo
			,mi.HostURL
			,mi.OriginalImgPath
			,0 RowOrder
		FROM Dealer_NewCar AS DNC WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsored DS WITH (NOLOCK) ON DNC.TcDealerId = DS.DealerId
	INNER JOIN pq_dealercitiesmodels PCM WITH (NOLOCK) ON PCM.CampaignId = DS.Id
	INNER JOIN CarMakes AS CMA  WITH (NOLOCK) ON CMA.ID = DNC.MakeId
	JOIN Cities AS C  WITH (NOLOCK) ON DNC.CityId = C.ID AND C.IsDeleted = 0
	JOIN  States AS S WITH (NOLOCK) ON C.StateId = S.Id AND S.IsDeleted = 0
	LEFT JOIN  Microsite_DealerContent mdc  WITH (NOLOCK) ON DNC.TcDealerId =  mdc.DealerId AND mdc.ContentCatagoryId = 2 AND mdc.IsActive = 1 and mdc.ContentSubCatagoryId is null
	LEFT JOIN Microsite_Images mi  WITH (NOLOCK) ON DNC.TcDealerId = mi.DealerId  AND mi.IsActive = 1 AND mi.IsBanner =1 and mi.BannerImgSortingOrder=0
	WHERE  DNC.MakeId = @MakeId
		AND PCM.CityId = @CityId
		AND CMA.IsDeleted = 0
		AND DNC.IsActive = 1
		AND DNC.IsPremium = 1
		AND DNC.IsNewDealer=1
		AND GETDATE() BETWEEN DNC.PackageStartDate and DNC.PackageEndDate
		AND (( DS.TotalCount < DS.TotalGoal AND DS.DailyCount < DS.DailyGoal)
			OR type <> 2) 
		AND Getdate() BETWEEN DS.StartDate AND DS.EndDate
		), CTE2 AS -- select * from cte 
	--UNION ALL
	(
	SELECT DISTINCT DNC.TcDealerId AS Id
		,DNC.Id NewCarDealerId
		,DNC.NAME AS DealerName
		,DNC.Address
		,DNC.PinCode
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite AS WebSite
		,DNC.WorkingHours
		,'+91' + DNC.PrimaryMobileNo AS MobileNo -- modified by sanjay on 27/2/2015
		,CMA.NAME AS CarMake
		,C.NAME AS City
		,S.NAME AS STATE
		,DNC.Latitude AS Latitude -- modeified by sanjay 23/2/15
		,DNC.Longitude AS Longitude -- modeified by sanjay 23/2/15
		,0 AS IsPremium
		,mdc.DealerContent
		,DNC.ShowroomStartTime AS StartTime
		,DNC.ShowroomEndTime AS EndTime
		,mi.HostURL + mi.DirectoryPath + mi.ImgPathCustom600 AS ShowroomImage
		,CMA.NAME AS MakeName
		,DNC.LandLineNo
		,mi.HostURL
		,mi.OriginalImgPath
		,ROW_NUMBER() OVER (PARTITION BY DNC.TcDealerId,DNC.ID   ORDER BY DNC.ID DESC ) RowOrder
	FROM Dealer_NewCar AS DNC WITH (NOLOCK)
	LEFT JOIN PQ_DealerSponsored DS WITH (NOLOCK) ON DNC.TcDealerId = DS.DealerId
	JOIN Cities AS C WITH (NOLOCK) ON DNC.CityId = C.ID
	JOIN States AS S WITH (NOLOCK)  ON C.StateId = S.Id
	JOIN CarMakes AS CMA  WITH (NOLOCK) ON DNC.MakeId = CMA.ID
	LEFT JOIN  Microsite_DealerContent mdc  WITH (NOLOCK)  ON DNC.TcDealerId =  mdc.DealerId AND mdc.ContentCatagoryId = 2 AND mdc.IsActive = 1 and mdc.ContentSubCatagoryId is null
	LEFT JOIN Microsite_Images mi  WITH (NOLOCK) ON DNC.TcDealerId = mi.DealerId  AND mi.IsActive = 1 AND mi.IsBanner =1 and mi.BannerImgSortingOrder=0
	WHERE DNC.CityId = @CityId
		AND DNC.MakeId = @MakeId
		AND DNC.IsActive = 1
		AND C.IsDeleted = 0
		AND S.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND DNC.IsNewDealer=1
		AND (                 
			DNC.IsPremium IS NULL 
			OR DNC.IsPremium = 0
			OR (DNC.IsPremium=1 AND (GETDATE() NOT BETWEEN DNC.PackageStartDate and DNC.PackageEndDate OR DNC.PackageEndDate is NULL OR DNC.PackageStartDate is null )
			OR (DNC.IsPremium=1 AND (DS.TotalCount >= DS.TotalGoal AND DS.DailyCount >= DS.DailyGoal)
			OR type = 2) 
		AND Getdate() < CONVERT(Datetime,IsNUll(DS.EndDate,'2099-12-31'))
			)
		 )
		 ) SELECT * FROM CTE WITH (NOLOCK)
		   UNION ALL
		   (SELECT * FROM CTE2 WITH (NOLOCK)
		   WHERE RowOrder=1)
		   	ORDER BY IsPremium DESC
END

