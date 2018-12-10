IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDetails_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDetails_V15]
GO

	-- =============================================          
-- Author:  <Vinayak>          
-- Description: <Get the dealer detail based on dealerId>
-- Modified By : Shalini Nair on 12/03/15 to retrieve IsPremium field
-- Modified By : Sanjay Soni on 14/10/2015 added Campaign ID in Output Parameter
-- Modified By : Shalini Nair on 19/11/2015 added @DealerCampaignId,@PQCity,@PQMakeId  as input parameters
-- =============================================          
CREATE PROCEDURE [dbo].[GetNCDDetails_V15.11.3] (
	@DealerId INT
	,
	--OUTPUT--
	@MakeId INT OUTPUT
	,@CityId INT OUTPUT
	,@Name VARCHAR(200) OUTPUT
	,@Address VARCHAR(400) OUTPUT
	,@Pincode VARCHAR(20) OUTPUT
	,@ContactNo VARCHAR(100) OUTPUT
	,@FaxNo VARCHAR(30) OUTPUT
	,@EMailId VARCHAR(100) OUTPUT
	,@WebSite VARCHAR(100) OUTPUT
	,@WorkingHours VARCHAR(50) OUTPUT
	,@HostURL VARCHAR(100) OUTPUT
	,@ContactPerson VARCHAR(100) OUTPUT
	,@DealerMobileNo VARCHAR(100) OUTPUT
	,@Mobile VARCHAR(100) OUTPUT --masked
	,@ShowroomStartTime VARCHAR(50) OUTPUT
	,@ShowroomEndTime VARCHAR(50) OUTPUT
	,@PrimaryMobileNo VARCHAR(50) OUTPUT
	,@SecondaryMobileNo VARCHAR(50) OUTPUT
	,@LandLineNo VARCHAR(50) OUTPUT
	,@DealerArea VARCHAR(400) OUTPUT
	,@Latitude FLOAT OUTPUT
	,@Longitude FLOAT OUTPUT
	,@CityName VARCHAR(50) OUTPUT
	,@StateName VARCHAR(30) OUTPUT
	,@CampaignId INT OUTPUT
	,@IsPremium BIT OUTPUT
	,@DealerCampaignId INT = NULL -- inserted for DealerShowroom on PQ 
	,@PQCityId INT = NULL -- inserted for DealerShowroom on PQ
	,@PQMakeId INT = NULL -- inserted for DealerShowroom on PQ
	)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @RandomId VARCHAR(100)

	SELECT  @MakeId = TDM.MakeId
		,@CityId = D.CityId
		,@Name = D.Organization
		,@Address = D.Address1 + ' ' + isnull(D.Address2, '')
		,@Pincode = D.Pincode
		,@ContactNo = D.MobileNo
		,@FaxNo = D.FaxNo
		,@EMailId = D.EMailId
		,@WebSite = D.WebsiteUrl
		,@WorkingHours = D.ContactHours
		,@HostURL = D.HostURL
		,@ContactPerson = D.ContactPerson
		,@DealerMobileNo = D.PhoneNo
		,@Mobile = DS.Phone
		,@ShowroomStartTime = D.ShowroomStartTime
		,@ShowroomEndTime = D.ShowroomEndTime
		,@PrimaryMobileNo = D.PhoneNo
		,@SecondaryMobileNo = D.MobileNo
		,@LandLineNo = D.MobileNo
		,@DealerArea = D.AreaId
		,@Latitude = D.Lattitude
		,@Longitude = D.Longitude
		,@CityName = ci.NAME
		,@StateName = s.NAME
		,@IsPremium = DNC.IsDealerLocatorPremium
		,@CampaignId = DNC.PQ_DealerSponsoredId
		,@RandomId = NEWID()
	FROM DealerLocatorConfiguration DNC WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsored DS WITH (NOLOCK) ON DS.Id = DNC.PQ_DealerSponsoredId
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
	INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
	LEFT JOIN cities ci WITH (NOLOCK) ON ci.id = D.CityId
	LEFT JOIN states s WITH (NOLOCK) ON ci.StateId = s.ID
	WHERE (
			@DealerCampaignId IS NOT NULL
			OR DNC.DealerId = @DealerId
			)
		AND (
			@DealerCampaignId IS NULL
			OR dnc.PQ_DealerSponsoredId = @DealerCampaignId
			)
		AND (
			@PQCityId IS NULL
			OR D.CityId = @PQCityId
			)
		AND (@PQMakeId IS NULL OR TDM.MakeId = @PQMakeId)
		AND DNC.IsLocatorActive = 1
		AND DNC.IsDealerLocatorPremium = 1
		AND DS.IsActive = 1
		AND D.IsDealerActive = 1
		AND dbo.IsCampaignActiveBasic(ds.Id) = 1
	ORDER BY NEWID()
END
