IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDealers_V1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDealers_V1]
GO

	
-- =============================================          
-- Author:  <supriya > 
-- Created On : 10/9/2014          
-- Description: Reference of the sp "GetNCDDealers" and made changes to get DealerContent,ShowroomImage,StartTime and EndTime      
-- =============================================          
CREATE PROCEDURE [dbo].[GetNCDDealers_V1.0] (
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
		,DNC.ContactNo
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite
		,DNC.WorkingHours
		,'+91 ' + DNC.Mobile AS MobileNo
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
		,DNC.ContactNo
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite
		,DNC.WorkingHours
		,DNC.DealerMobileNo AS MobileNo
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


