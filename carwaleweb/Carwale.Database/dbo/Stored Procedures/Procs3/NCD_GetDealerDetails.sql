IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_GetDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_GetDealerDetails]
GO

	-- =============================================
-- Author	:	Sachin Bharti(19th Aug 2014)
-- Description	:	Get dealer details from Dealers 
--					and Dealer_NewCar table for Dealer locator Page.
-- Execute [dbo].[NCD_GetDealerDetails] 6701,1622,10052
-- =============================================
CREATE PROCEDURE [dbo].[NCD_GetDealerDetails]
	@TC_DealerId	INT,
	@NCD_DealerId	INT = NULL,
	@CampaignId		INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CityId VARCHAR(50) = NULL

	SELECT 	D.ID AS DealerID,D.Organization,
			D.MobileNo AS Mobile ,D.PhoneNo AS LandlineNumber,
			D.ContactPerson,D.ContactHours,
			D.WebsiteUrl AS Website,D.IsDealerActive AS IsActive, D.IsPremium , D.HavingWebsite,
			D.StateId ,D.CityId , D.AreaId,D.Address1 AS Address,D.Pincode,
			D.EmailId , D.Longitude,D.Lattitude ,D.FaxNo,
			ISNULL(DNC.MakeId,-1) AS MakeId ,ISNULL(CM.Name,'All Makes') AS MakeName ,
			DNC.Mobile AS MaskingNumber,
			ISNULL(DNC.ShowroomStartTime,'') AS StartTime ,
			ISNULL(DNC.ShowroomEndTime,'') AS EndTime

	FROM Dealers D(NOLOCK)
	INNER	JOIN RVN_DealerPackageFeatures RVN(NOLOCK) ON RVN.DealerId = D.ID AND RVN.DealerPackageFeatureID = @CampaignId 
	INNER	JOIN States S(NOLOCK) ON S.ID = D.StateId AND S.IsDeleted = 0
	LEFT	JOIN Dealer_NewCar DNC(NOLOCK) ON DNC.TcDealerId = D.ID AND DNC.Id = @NCD_DealerId AND DNC.CampaignId = @CampaignId
	LEFT	JOIN CarMakes CM(NOLOCK) ON CM.ID = DNC.MakeId
	
	WHERE D.ID = @TC_DealerId

	SELECT @CityId = D.CityId FROM Dealers D(NOLOCK) WHERE D.ID = @TC_DealerId

END
