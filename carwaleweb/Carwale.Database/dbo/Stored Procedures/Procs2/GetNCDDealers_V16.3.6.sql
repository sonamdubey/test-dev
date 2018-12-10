IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDealers_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDealers_V16]
GO

	-- =============================================          
-- Author:  <supriya > exec [GetNCDDealers_V16.3.6] -1,1,18
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
-- Modified by Vinayak on 18/11/2015 removing Dealer_NewCar dependency
-- Modified by Vicky on 18/11/2015 updated the changes for removing Dealer_NewCar dependency and implemented IsCampaignActiveBasic function for check active campaign
-- Modified by Vicky on 24/11/2015 inserted stateId parameter for Dealer locator pages revamp
-- Modified by Rohan on 28/12/2015 Added Profilepic,hosturl,firstname,lastname in select statements
-- Modified by Ashwini Todkar on 4 Jan 2016 -retrieved leadpanel,leadbusinesstype and showemail
-- Modified by Shalini on 04/01/2016 retrieving Masking number from MM_SellerMobileMasking 
-- Modified by Shalini Nair on 29/01/2016 to retrieve CarwaleTollFree number based on IsDefaultnumber column
-- Modified by Vicky Lund on 19/02/2016 to use contract parameters (vwActiveCampaigns) instead of campaign parameters
-- Modified by Sanjay Soni on 29/02/2016 remove masking number filter condition 
-- Modified by Vicky Lund on 16/03/2016, removed Address2
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
-- =============================================          
CREATE PROCEDURE [dbo].[GetNCDDealers_V16.3.6] (
	@StateId SMALLINT
	,@CityId SMALLINT
	,@MakeId SMALLINT
	)
AS
BEGIN
	SET NOCOUNT ON;

	WITH cte
	AS (
		SELECT D.Id AS Id
			,D.Id AS NewCarDealerId
			,D.Organization AS DealerName
			,D.Address1 AS Address
			,D.PinCode
			,D.FaxNo
			,D.EMailId
			,D.WebsiteUrl AS WebSite
			,CampaignMobileNo = CASE 
				WHEN P.IsDefaultNumber != 0
					THEN CTOLL.TollFreeNumber
				ELSE MM.MaskingNumber
				END
			,D.MobileNo DealerLevelMobileNo
			,C.NAME AS City
			,C.ID AS CityId
			,S.NAME AS STATE
			,S.ID AS StateId
			,D.Lattitude AS Latitude
			,D.Longitude AS Longitude
			,D.ProfilePhotoHostUrl
			,D.ProfilePhotoUrl
			,D.FirstName
			,D.LastName
			,CASE 
				WHEN VRC.CampaignId IS NOT NULL
					THEN 1
				ELSE 0
				END AS IsPremium
			,D.ShowroomStartTime AS StartTime
			,D.ShowroomEndTime AS EndTime
			,ISNULL(mi.HostURL, '') + ISNULL(mi.DirectoryPath, '') + ISNULL(mi.ImgPathCustom600, '') AS ShowroomImage
			,CMA.NAME AS MakeName
			,D.PhoneNo AS LandLineNo
			,DNC.PQ_DealerSponsoredId AS CampaignId
			,P.ShowEmail
			,P.LeadPanel
			,D.DealerLeadBusinessType
			,mi.HostURL
			,mi.OriginalImgPath
			,mdc.DealerContent
			,ROW_NUMBER() OVER (
				ORDER BY NEWID()
				) RowOrder
		FROM DealerLocatorConfiguration AS DNC WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
		INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
		JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
		JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
		JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
		LEFT JOIN PQ_DealerSponsored AS P WITH (NOLOCK) ON DNC.PQ_DealerSponsoredId = P.Id
			AND P.IsActive = 1
		LEFT OUTER JOIN vwRunningCampaigns AS VRC WITH (NOLOCK) ON P.Id = VRC.CampaignId
		LEFT JOIN Microsite_DealerContent mdc WITH (NOLOCK) ON D.Id = mdc.DealerId
			AND mdc.ContentCatagoryId = 2
			AND mdc.IsActive = 1
			AND mdc.ContentSubCatagoryId IS NULL
		LEFT JOIN Microsite_Images mi WITH (NOLOCK) ON DNC.DealerId = mi.DealerId
			AND mi.IsActive = 1
			AND mi.IsBanner = 1
			AND mi.BannerImgSortingOrder = 0
		LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = P.Id
			AND MM.ApplicationId = 1
		LEFT JOIN CarwaleTollFreeNumber CTOLL WITH (NOLOCK) ON P.IsDefaultNumber = CTOLL.Id
		WHERE (
				D.CityId = @CityId
				OR @CityId = - 1
				)
			AND (
				C.StateId = @StateId
				OR @StateId = - 1
				)
			AND TDM.MakeId = @MakeId
			AND DNC.IsLocatorActive = 1
			AND C.IsDeleted = 0
			AND S.IsDeleted = 0
			AND CMA.IsDeleted = 0
			AND DNC.IsDealerLocatorPremium = 1
			AND D.TC_DealerTypeId = 2
			AND D.IsDealerActive = 1
		)
	SELECT Id
		,NewCarDealerId
		,DealerName
		,Address
		,PinCode
		,FaxNo
		,EMailId
		,WebSite
		,CASE 
			WHEN IsPremium = 1
				THEN CampaignMobileNo
			ELSE DealerLevelMobileNo
			END AS MobileNo
		,City
		,CityId
		,STATE
		,StateId
		,Latitude
		,Longitude
		,IsPremium
		,StartTime
		,EndTime
		,ShowroomImage
		,MakeName
		,LandLineNo
		,CampaignId
		,ShowEmail
		,LeadPanel
		,DealerLeadBusinessType
		,HostURL
		,OriginalImgPath
		,DealerContent
		,ProfilePhotoHostUrl
		,ProfilePhotoUrl
		,FirstName
		,LastName
		,RowOrder
	FROM cte
	
	UNION ALL
	
	SELECT D.Id AS Id
		,D.Id AS NewCarDealerId
		,D.Organization AS DealerName
		,D.Address1 AS Address
		,D.PinCode
		,D.FaxNo
		,D.EMailId
		,D.WebsiteUrl AS WebSite
		,'+91' + D.MobileNo AS MobileNo
		,C.NAME AS City
		,C.ID AS CityId
		,S.NAME AS STATE
		,S.ID AS StateId
		,D.Lattitude AS Latitude
		,D.Longitude AS Longitude
		,0 AS IsPremium
		,D.ShowroomStartTime AS StartTime
		,D.ShowroomEndTime AS EndTime
		,ISNULL(mi.HostURL, '') + ISNULL(mi.DirectoryPath, '') + ISNULL(mi.ImgPathCustom600, '') AS ShowroomImage
		,CMA.NAME AS MakeName
		,D.PhoneNo AS LandLineNo
		,- 1 AS CampaignId
		,0 AS ShowEmail
		,- 1 AS LeadPanel
		,- 1 AS DealerLeadBusinessType
		,mi.HostURL
		,mi.OriginalImgPath
		,mdc.DealerContent
		,NULL AS ProfilePhotoHostUrl
		,NULL AS ProfilePhotoUrl
		,D.FirstName
		,D.LastName
		,ROW_NUMBER() OVER (
			ORDER BY D.Organization
			) RowOrder
	FROM DealerLocatorConfiguration AS DNC WITH (NOLOCK)
	JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
	JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
	JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
	JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
	JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
	LEFT JOIN Microsite_DealerContent mdc WITH (NOLOCK) ON D.Id = mdc.DealerId
		AND mdc.ContentCatagoryId = 2
		AND mdc.IsActive = 1
		AND mdc.ContentSubCatagoryId IS NULL
	LEFT JOIN Microsite_Images mi WITH (NOLOCK) ON DNC.DealerId = mi.DealerId
		AND mi.IsActive = 1
		AND mi.IsBanner = 1
		AND mi.BannerImgSortingOrder = 0
	WHERE (
			D.CityId = @CityId
			OR @CityId = - 1
			)
		AND (
			C.StateId = @StateId
			OR @StateId = - 1
			)
		AND TDM.MakeId = @MakeId
		AND C.IsDeleted = 0
		AND S.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND D.TC_DealerTypeId = 2
		AND D.IsDealerActive = 1
		AND DNC.IsLocatorActive = 1
		AND DNC.IsDealerLocatorPremium = 0
	ORDER BY IsPremium DESC
		,RowOrder
END
