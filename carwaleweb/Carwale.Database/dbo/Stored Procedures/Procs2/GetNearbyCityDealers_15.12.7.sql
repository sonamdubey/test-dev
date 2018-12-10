IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNearbyCityDealers_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNearbyCityDealers_15]
GO

	
-- =============================================
-- Author:		<Ashwini Todkar>
-- Create date: <25/8/2014>
-- Description:	<Gets the Nearby cities dealers for a particular city>
-- Changed SELECT Fields
-- =============================================
CREATE PROCEDURE [dbo].[GetNearbyCityDealers_15.12.7] --[dbo].[GetNearbyCityDealers_15.12.7] 10,'11658,5' [dbo].[GetNearbyCityDealers_15.12.7] 1,'11658,11658,11658,9350,15851,7803,15729'
	@CityId INT
	,@DealerIds VARCHAR(50)
AS
BEGIN
	DECLARE @UserLatitude NUMERIC
		,@UserLongitude NUMERIC
		,@NearestCity INT = NULL
		,@ConstLt numeric(12,9) = 0.030694236 -- // constant (km per 1 unit)
		,@ConstLg numeric(12,9) = 0.028870889 -- // constant (km per 1 unit)

	SELECT @UserLatitude = c.Lattitude
		,@UserLongitude = c.Longitude
	FROM Cities c WITH (NOLOCK)
	WHERE id = @CityId

	DECLARE @Latt DECIMAL = ABS(@UserLatitude)
		,@Long DECIMAL = ABS(@UserLongitude);

	WITH CTE
	AS (
		SELECT TOP 1 CT.ID AS CityId
			,Sqrt(Power(((ISNULL(@Latt, 0) - CT.Lattitude) * @ConstLt), 2) + Power(((ISNULL(@Long, 0) - CT.Longitude) * @ConstLg), 2)) AS Distance
		FROM Cities CT WITH (NOLOCK)
		JOIN Dealers d WITH (NOLOCK) ON CT.ID = d.CityId
			AND D.IsDealerActive = 1
		WHERE d.id IN (
				SELECT ListMember
				FROM fnSplitCSV(@DealerIds)
				)
			AND CT.IsDeleted = 0
		ORDER BY Distance
		)
	SELECT @NearestCity = CTE.CityId
	FROM CTE;

WITH cte
	AS (
		SELECT D.Id AS Id
			,D.Id AS NewCarDealerId -- Added by Manish on 23-10-2015
			,D.Organization AS DealerName
			,D.Address1 + ' ' + isnull(D.Address2, '') AS Address
			,D.PinCode
			,D.FaxNo
			,D.EMailId
			,D.WebsiteUrl AS WebSite
			--,D.WorkingHours
			,P.Phone CampaignMobileNo
			,D.MobileNo DealerLevelMobileNo
			--END AS MobileNo 
			,C.NAME AS City
			,C.Id as CityId
			,S.NAME AS STATE
			,S.ID AS StateId
			,D.Lattitude AS Latitude
			,D.Longitude AS Longitude
			,D.ProfilePhotoHostUrl
			,D.ProfilePhotoUrl
			,D.FirstName
			,D.LastName
			,CASE 
				WHEN [dbo].[IsCampaignActiveBasic](P.Id) = 1
					THEN 1
				ELSE 0
				END AS IsPremium
			,D.ShowroomStartTime AS StartTime
			,D.ShowroomEndTime AS EndTime
			,mi.HostURL + mi.DirectoryPath + mi.ImgPathCustom600 AS ShowroomImage
			--,CMA.NAME AS MakeName
			,D.PhoneNo AS LandLineNo
			,DNC.PQ_DealerSponsoredId AS CampaignId
			,mi.HostURL
			,mi.OriginalImgPath
			,mdc.DealerContent
			,ROW_NUMBER() OVER (
				ORDER BY NEWID()
				) RowOrder
		FROM
		Dealers D WITH (NOLOCK) 
		JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
		JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
		--INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID		
		--JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
		--JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
		LEFT JOIN DealerLocatorConfiguration AS DNC WITH (NOLOCK) ON D.ID = DNC.DealerId
		LEFT JOIN PQ_DealerSponsored AS P WITH (NOLOCK) ON DNC.PQ_DealerSponsoredId = P.Id
			AND P.IsActive = 1
		LEFT JOIN Microsite_DealerContent mdc WITH (NOLOCK) ON D.Id = mdc.DealerId
			AND mdc.ContentCatagoryId = 2
			AND mdc.IsActive = 1
			AND mdc.ContentSubCatagoryId IS NULL
		LEFT JOIN Microsite_Images mi WITH (NOLOCK) ON D.ID = mi.DealerId
			AND mi.IsActive = 1
			AND mi.IsBanner = 1
			AND mi.BannerImgSortingOrder = 0
		WHERE D.CityId = @NearestCity
			AND C.IsDeleted = 0
			--AND D.TC_DealerTypeId = 2
			AND D.IsDealerActive = 1
			AND D.Id IN (
			SELECT ListMember
			FROM fnSplitCSV(@DealerIds)
			)
		)
	SELECT Id
		,NewCarDealerId -- Added by Manish on 23-10-2015
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
		,FirstName
		,LastName
		,IsPremium
		,StartTime
		,EndTime
		,ShowroomImage
		--,MakeName
		,LandLineNo
		,CampaignId
		,HostURL
		,OriginalImgPath
		,DealerContent
		,RowOrder
		,ProfilePhotoHostUrl
		,ProfilePhotoUrl
	FROM cte
END
