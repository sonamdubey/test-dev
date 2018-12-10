IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerDetails_08012016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerDetails_08012016]
GO

	
-- =============================================
-- Author:		ASHWINI TODKAR
-- Create date: 27th OCT 2014
-- Description:	PROC TO GET DEALER DETAILS AND ALSO OFFERS
-- EXEC BW_GetDealerDetails_08012016 4,164,1
-- Modified By : Sadhana Upadhyay on 12 Nov 2014
-- Summary : to filter expire offers
-- Modified By : Ashwini Todkar on 20 Nov 2014 
-- Modified By : Sadhana Upadhyay on 3rd Dec 2014
-- Summary : Retrieved Loan provider
-- Modified By : Ashwini Todkar on 15 dec 2014
-- Summary : Added select clause to get bike booking amount
-- Modified By : Ashish G. Kamble on 24 June 2015
-- Modified : BW_GetDealerPriceQuote sp modified to get the offers data. Query to get the offers removed from this sp.
-- Modified By : Sangram Nandkhile on 08 Jan 2016
-- Modified : Modified to cater isPriceImpact column
-- Modified By : Sushil Kumar
-- Summary : Add provision to fetch bike availability by color 
-- Modified By : Sadhana Upadhyay on 12 Jan 2016
-- Summary : to get availability by color
-- Modified By	:	Sumit Kate on 30 Mar 2016
-- Summary	:	Get the Dealer Name/Email/Contact from the active campaign
-- Modified By	:	Sumit Kate on 15 Apr 2016
-- Description	:	refer ContractStatus instead of IsActiveContract
-- EXEC [dbo].[BW_GetDealerDetails_08012016] 18990,832,1
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerDetails_08012016]
	-- Add the parameters for the stored procedure here
	@DealerId int
	,@VersionId int
	,@CityId int
AS
BEGIN	
	SET NOCOUNT ON;

	DECLARE @ModelId INT

	SELECT @ModelId = BV.BikeModelId
	FROM BikeVersions BV WITH (NOLOCK)
	WHERE BV.IsDeleted = 0
		AND BV.New = 1
		AND BV.ID = @VersionId

	EXEC BW_GetDealerPriceQuote_08012016 @CityId
		,@VersionId
		,@DealerId
		
	SELECT 
		D.ID
		,D.FirstName
		,D.LastName
		,D.Address1 AS Address
		,campaign.Number AS MobileNo 
		,D.MobileNo AS PhoneNo
		,ISNULL(D.WeAreOpen, 0) WeAreOpen
		,D.ContactHours
		,A.NAME AS AreaName
		,C.NAME AS CityName
		,S.NAME AS StateName
		,ISNULL(D.Lattitude, 0) Lattitude
		,ISNULL(D.Longitude, 0) Longitude
		,D.AreaId
		,D.Pincode
		,campaign.DealerEmailId AS EmailId
		,D.WebsiteUrl
		,campaign.DealerName AS Organization
	FROM Dealers AS D WITH (NOLOCK)
	INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = D.AreaId
	INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId
	INNER JOIN States AS S WITH (NOLOCK) ON S.ID = C.StateId	
	INNER JOIN BW_PQ_DealerCampaigns campaign WITH(NOLOCK) ON d.IsDealerActive = 1 and d.IsDealerDeleted = 0 and d.ApplicationId = 2 AND d.Id = campaign.DealerId AND campaign.IsActive = 1
	INNER JOIN TC_ContractCampaignMapping contractCampaign WITH(NOLOCK) ON campaign.ContractId = contractCampaign.ContractId AND contractCampaign.ContractStatus = 1 AND campaign.Id = contractCampaign.CampaignId
	INNER JOIN BW_PQ_CampaignRules rules WITH(NOLOCK) ON campaign.Id = rules.CampaignId AND rules.IsActive = 1 
	AND rules.ModelId = @modelId AND rules.CityId = @cityId	
	INNER JOIN ContractStatus CS WITH(NOLOCK) ON CS.Id = contractCampaign.ContractStatus 
	INNER JOIN RVN_DealerPackageFeatures RDP WITH(NOLOCK) ON RDP.DealerPackageFeatureID = contractCampaign.ContractId
	INNER JOIN Packages P WITH(NOLOCK) ON P.Id = RDP.PackageId
	WHERE D.ID = @DealerId 
		AND D.IsDealerDeleted = 0


	--Get Facilities offered by dealer
	SELECT DF.Facility
	FROM BW_DealerFacilities DF WITH (NOLOCK)
	WHERE DF.DealerId = @DealerId
		AND DF.IsActive = 1

	--Get EMI details offered by dealer
	SELECT LA.Id
		,ISNULL(LA.LTV, 0) LTV
		,ISNULL(LA.Tenure, 0) Tenure
		,ISNULL(LA.RateOfInterest, 0) RateOfInterest
		,LA.LoanProvider
	FROM BW_DealerLoanAmounts LA WITH (NOLOCK)
	WHERE LA.DealerId = @DealerId

	--Get Booking Amount of a bike added by Ashwini
	SELECT ISNULL(BA.Amount,0) Amount FROM BW_DealerBikeBookingAmounts BA WITH(NOLOCK)
	INNER JOIN BikeVersions BV WITH(NOLOCK)
	ON BV.ID = BA.VersionId
	WHERE BA.VersionId = @VersionId 
	AND BA.DealerId = @DealerId
	AND BA.IsActive = 1

	--Get BikeAvailabilityByColor added by sushil kumar
	SELECT BVC.ModelColorID AS ColorId
		,BMC.ColorName
		,ISNULL(BAC.NumOfDays,0 ) AS NumOfDays
		,BCC.HexCode
		,ISNULL(BAC.DealerId,0) AS DealerId
		,ISNULL(BAC.BikeVersionId, 0) AS BikeVersionId
	FROM BikeVersionColorsMaster BVC WITH (NOLOCK)
	INNER JOIN BikeModelColors BMC WITH (NOLOCK) ON BMC.ID = BVC.ModelColorID
		AND bmc.IsActive = 1
	LEFT JOIN BikeColorCodes BCC WITH (NOLOCK) ON BCC.ModelColorID = BMC.ID
		AND BCC.IsActive = 1
	LEFT JOIN BW_BikeAvailabilityByColor BAC WITH (NOLOCK) ON BAC.ColorId = BMC.ID
		AND BVC.VersionId = BAC.BikeVersionId
	WHERE BAC.DealerId=@DealerId AND BAC.BikeVersionId = @VersionId
		AND BVC.IsActive = 1
END
