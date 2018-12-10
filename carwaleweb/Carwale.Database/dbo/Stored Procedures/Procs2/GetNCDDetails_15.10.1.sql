IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDetails_15]
GO

	
-- =============================================          
-- Author:  <Vinayak>          
-- Description: <Get the dealer detail based on dealerId>
-- Modified By : Shalini Nair on 12/03/15 to retrieve IsPremium field
-- Modified By : Sanjay Soni on 14/10/2015 added Campaign ID in Output Parameter
-- =============================================          
CREATE  PROCEDURE [dbo].[GetNCDDetails_15.10.1] (
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
	,@Mobile VARCHAR(100) OUTPUT
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
	)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT @MakeId = DNC.MakeId
		,@CityId = DNC.CityId
		,@Name = DNC.NAME
		,@Address = DNC.Address
		,@Pincode = DNC.Pincode
		,@ContactNo = DNC.ContactNo
		,@FaxNo = DNC.FaxNo
		,@EMailId = DNC.EMailId
		,@WebSite = DNC.WebSite
		,@WorkingHours = DNC.WorkingHours
		,@HostURL = DNC.HostURL
		,@ContactPerson = DNC.ContactPerson
		,@DealerMobileNo = DNC.DealerMobileNo
		,@Mobile = DS.Phone
		,@ShowroomStartTime = DNC.ShowroomStartTime
		,@ShowroomEndTime = DNC.ShowroomEndTime
		,@PrimaryMobileNo = DNC.PrimaryMobileNo
		,@SecondaryMobileNo = DNC.SecondaryMobileNo
		,@LandLineNo = DNC.LandLineNo
		,@DealerArea = DNC.DealerArea
		,@Latitude = DNC.Latitude
		,@Longitude = DNC.Longitude
		,@CityName = ci.NAME
		,@StateName = s.NAME
		,@IsPremium = DNC.IsPremium
		,@CampaignId = DNC.PqCampaignId
	FROM Dealer_NewCar DNC WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsored DS WITH (NOLOCK) ON DS.Id = DNC.PqCampaignId
	INNER JOIN cities ci WITH (NOLOCK) ON ci.id = DNC.CityId
	INNER JOIN states s WITH (NOLOCK) ON ci.StateId = s.ID
	WHERE DNC.TcDealerId = @DealerId
		AND DNC.IsActive = 1
		AND DNC.IsNewDealer = 1
		AND DS.IsActive = 1
		AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, DNC.PackageStartDate)
			AND CONVERT(DATE, ISNULL(DNC.PackageEndDate, '12-31-2099'))
		AND (
			(
				DS.EndDate IS NULL
				AND DS.TotalCount < DS.TotalGoal
				AND DS.DailyCount < IsNull(DS.DailyGoal, DS.TotalCount)
				) --Lead Count Based
			OR (
				DS.EndDate IS NOT NULL
				AND CONVERT(DATE, GETDATE()) BETWEEN DS.StartDate
					AND CONVERT(DATETIME, IsNUll(DS.EndDate, '2099-12-31'))
				) --Time Duration Based
			)
END


