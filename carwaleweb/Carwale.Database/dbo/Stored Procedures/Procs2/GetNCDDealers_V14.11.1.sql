IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDealers_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDealers_V14]
GO

	
-- =============================================          
-- Author:  <supriya > 
-- Created On  : 10/10/2014          
-- Description : Reference of the sp "GetNCDDealers" and made changes to get DealerContent,ShowroomImage,StartTime and EndTime
-- Description : Reference of the sp [dbo].[GetNCDDealers_V1.0]
-- Created by Supriya Khartode on 10/10/2014 to fetch mobileno from PrimaryMobileNo column 
-- Modified by Supriya Khartode on 4/11/2014 to fetch makename & landlineno column   
-- =============================================          
CREATE PROCEDURE [dbo].[GetNCDDealers_V14.11.1] (
	@CityId SMALLINT
	,@MakeId SMALLINT
	)
AS
BEGIN
	SET NOCOUNT ON;

	with cte as 
(	SELECT DNC.TcDealerId AS Id
		,DNC.NAME AS DealerName
		,DNC.Address
		,DNC.PinCode
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite
		,DNC.WorkingHours
		,'+91' + DNC.PrimaryMobileNo AS MobileNo
		,CMA.NAME AS CarMake
		,C.NAME AS City
		,S.NAME AS STATE
		,NCD_Website
		,Nd.Lattitude AS Latitude
		,Nd.Longitude AS Longitude
		,1 AS IsPremium
		,mdc.DealerContent
		,DNC.ShowroomStartTime AS StartTime
		,DNC.ShowroomEndTime AS EndTime
		,mi.HostURL+mi.DirectoryPath+mi.ImgPathCustom600 as ShowroomImage
		,CMA.Name AS MakeName
		,DNC.LandLineNo
		, ROW_NUMBER() OVER(ORDER BY NEWID()) RowOrder
	FROM Dealer_NewCar AS DNC WITH (NOLOCK)
	LEFT JOIN NCD_Dealers Nd WITH (NOLOCK) ON DNC.Id = Nd.DealerId
	--AND Nd.IsActive =1   Commented by Raghu   
	JOIN Cities AS C  WITH (NOLOCK) ON DNC.CityId = C.ID
	JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
	JOIN CarMakes AS CMA  WITH (NOLOCK) ON DNC.MakeId = CMA.ID
	LEFT JOIN  Microsite_DealerContent mdc  WITH (NOLOCK) ON DNC.TcDealerId =  mdc.DealerId AND mdc.ContentCatagoryId = 2 AND mdc.IsActive = 1
	LEFT JOIN Microsite_Images mi  WITH (NOLOCK) ON DNC.TcDealerId = mi.DealerId  AND mi.IsActive = 1 AND mi.IsBanner =1 and mi.BannerImgSortingOrder=0
	WHERE DNC.CityId = @CityId
		AND DNC.MakeId = @MakeId
		AND DNC.IsActive = 1
		AND C.IsDeleted = 0
		AND S.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND Nd.IsPremium = 1
		AND DNC.IsNewDealer=1 -- Avishkar Modified 10-07-2014
		) select * from cte 
	UNION ALL

	SELECT DNC.TcDealerId AS Id
		,DNC.NAME AS DealerName
		,DNC.Address
		,DNC.PinCode
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite
		,DNC.WorkingHours
		,'+91' + DNC.PrimaryMobileNo AS MobileNo
		,CMA.NAME AS CarMake
		,C.NAME AS City
		,S.NAME AS STATE
		,NCD_Website
		,Nd.Lattitude AS Latitude
		,Nd.Longitude AS Longitude
		,0 AS IsPremium
		,mdc.DealerContent
		,DNC.ShowroomStartTime AS StartTime
		,DNC.ShowroomEndTime AS EndTime
		,mi.HostURL+mi.DirectoryPath+mi.ImgPathCustom600 as ShowroomImage
		,CMA.Name AS MakeName
		,DNC.LandLineNo
		,ROW_NUMBER () OVER (ORDER BY  DNC.IsPriority DESC,DNC.NAME) RowOrder
	FROM Dealer_NewCar AS DNC WITH (NOLOCK)
	LEFT JOIN NCD_Dealers Nd WITH (NOLOCK) ON DNC.Id = Nd.DealerId
		AND Nd.IsActive = 1
	JOIN Cities AS C WITH (NOLOCK) ON DNC.CityId = C.ID
	JOIN States AS S WITH (NOLOCK)  ON C.StateId = S.Id
	JOIN CarMakes AS CMA  WITH (NOLOCK) ON DNC.MakeId = CMA.ID
	LEFT JOIN  Microsite_DealerContent mdc  WITH (NOLOCK)  ON DNC.TcDealerId =  mdc.DealerId AND mdc.ContentCatagoryId = 2 AND mdc.IsActive = 1
	LEFT JOIN Microsite_Images mi  WITH (NOLOCK) ON DNC.TcDealerId = mi.DealerId  AND mi.IsActive = 1 AND mi.IsBanner =1 and mi.BannerImgSortingOrder=0
	WHERE DNC.CityId = @CityId
		AND DNC.MakeId = @MakeId
		AND DNC.IsActive = 1
		AND C.IsDeleted = 0
		AND S.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND DNC.IsNewDealer=1 -- Avishkar Modified 10-07-2014
		AND (
			Nd.IsPremium IS NULL
			OR Nd.IsPremium = 0
			)
			ORDER BY IsPremium DESC ,RowOrder
END



