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
-- Modified By Shalini Nair on 12/08/15 retrieving originalImgPath
-- Modified By Sanjay Soni on 14/10/15 retrieve premium dealer locator based on Campaign exists 
-- Modified by Manish on 23-10-2015 added DNC.Id as NewCarDealerId
-- Modified by Sanjay on 3/11/2015 added condition in non premium section checking expiry date
-- =============================================          
CREATE PROCEDURE [dbo].[GetNCDDealers_V15.11.1.2] (
	@CityId SMALLINT
	,@MakeId SMALLINT
	)
AS
BEGIN
	SET NOCOUNT ON;

	with cte as 
(	SELECT DNC.TcDealerId AS Id
		,DNC.ID AS NewCarDealerId  -- Added by Manish on 23-10-2015
		,DNC.NAME AS DealerName
		,DNC.Address
		,DNC.PinCode
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite AS WebSite
		,DNC.WorkingHours
		,CASE 
			WHEN (P.IsActive = 1	AND (	
									(P.EndDate IS NULL AND P.TotalCount < P.TotalGoal AND P.DailyCount < IsNull(P.DailyGoal,P.TotalCount))--Lead Count Based
									OR  
									( P.EndDate IS NOT NULL AND CONVERT(date,GETDATE()) BETWEEN P.StartDate AND CONVERT(Datetime,IsNUll(P.EndDate,'2099-12-31')))--Time Duration Based
							    )) THEN 
				P.Phone 
			ELSE 
				'+91' + DNC.PrimaryMobileNo 
		END AS MobileNo 
		,CMA.NAME AS CarMake
		,C.NAME AS City
		,S.NAME AS STATE
		,DNC.Latitude AS Latitude 
		,DNC.Longitude AS Longitude 
		,CASE
			WHEN (P.IsActive = 1 AND (	
										(P.EndDate IS NULL AND P.TotalCount < P.TotalGoal AND P.DailyCount < IsNull(P.DailyGoal,P.TotalCount))--Lead Count Based
										OR  
										( P.EndDate IS NOT NULL AND CONVERT(date,GETDATE()) BETWEEN P.StartDate AND CONVERT(Datetime,IsNUll(P.EndDate,'2099-12-31')))--Time Duration Based
									)) THEN  
				1 
            ELSE 
				0 
			END AS IsPremium
		,mdc.DealerContent
		,DNC.ShowroomStartTime AS StartTime
		,DNC.ShowroomEndTime AS EndTime
		,mi.HostURL+mi.DirectoryPath+mi.ImgPathCustom600 as ShowroomImage
		,CMA.Name AS MakeName
		,DNC.LandLineNo
		,mi.HostURL 
		,mi.OriginalImgPath
		, ROW_NUMBER() OVER(ORDER BY NEWID()) RowOrder
	FROM Dealer_NewCar AS DNC WITH (NOLOCK) 
	JOIN PQ_DealerSponsored AS P WITH (NOLOCK) ON DNC.PqCampaignId = P.Id
	JOIN Cities AS C  WITH (NOLOCK) ON DNC.CityId = C.ID
	JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
	JOIN CarMakes AS CMA  WITH (NOLOCK) ON DNC.MakeId = CMA.ID
	LEFT JOIN  Microsite_DealerContent mdc  WITH (NOLOCK) ON DNC.TcDealerId =  mdc.DealerId AND mdc.ContentCatagoryId = 2 AND mdc.IsActive = 1 and mdc.ContentSubCatagoryId is null
	LEFT JOIN Microsite_Images mi  WITH (NOLOCK) ON DNC.TcDealerId = mi.DealerId  AND mi.IsActive = 1 AND mi.IsBanner =1 and mi.BannerImgSortingOrder=0
	WHERE DNC.CityId = @CityId
		AND DNC.MakeId = @MakeId
		AND DNC.IsActive = 1
		AND C.IsDeleted = 0
		AND S.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND DNC.IsPremium = 1
		AND DNC.IsNewDealer=1 
		AND GETDATE() BETWEEN DNC.PackageStartDate and ISNULL(DNC.PackageEndDate,'12-31-2099')
		) select * from cte  
	UNION ALL

	SELECT DNC.TcDealerId AS Id
		,DNC.ID AS NewCarDealerId  -- Added by Manish on 23-10-2015
		,DNC.NAME AS DealerName
		,DNC.Address
		,DNC.PinCode
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite AS WebSite
		,DNC.WorkingHours
		,'+91' + DNC.PrimaryMobileNo AS MobileNo
		,CMA.NAME AS CarMake
		,C.NAME AS City
		,S.NAME AS STATE
		,DNC.Latitude AS Latitude
		,DNC.Longitude AS Longitude
		,0 AS IsPremium
		,mdc.DealerContent
		,DNC.ShowroomStartTime AS StartTime
		,DNC.ShowroomEndTime AS EndTime
		,mi.HostURL+mi.DirectoryPath+mi.ImgPathCustom600 as ShowroomImage
		,CMA.Name AS MakeName
		,DNC.LandLineNo
		,mi.HostURL 
		,mi.OriginalImgPath
		,ROW_NUMBER () OVER (ORDER BY  DNC.IsPriority DESC,DNC.NAME) RowOrder
	FROM Dealer_NewCar AS DNC WITH (NOLOCK)
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
			OR    (DNC.IsPremium=1 AND  ISNULL(DNC.PackageEndDate,'12-31-2099')<GETDATE() )
			)
			ORDER BY IsPremium DESC ,RowOrder
END

