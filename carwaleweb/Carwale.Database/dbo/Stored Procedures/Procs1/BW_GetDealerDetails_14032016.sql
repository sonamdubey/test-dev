IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerDetails_14032016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerDetails_14032016]
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
-- Modified By	:	Sumit Kate on 14 Mar 2016
-- Summary	:	Return the data for Dealer Price Quote Page
-- Modified By	:	Sumit Kate on 15 Apr 2016
-- Description	:	refer ContractStatus instead of IsActiveContract
-- Modified by	:	Sumit Kate on 28 Apr 2016
-- Summary		:	Added Random sort on final result
-- Modified by	:	Sumit Kate on 23 Jun 2016
-- Summary		:	Show Secondary Dealers based on model
-- E.g. : EXEC BW_GetDealerDetails_14032016 null,112,1
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerDetails_14032016]
	-- Add the parameters for the stored procedure here
	@DealerId int = NULL
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
	
	--Bike Details
	SELECT 
		BM.Id AS MakeId
		,BM.NAME AS MakeName
		,BMO.ID AS ModelId
		,BMO.NAME AS ModelName
		,BV.ID AS VersionId
		,BV.NAME AS VersionName
		,BM.MaskingName AS MakeMaskingName
		,BMO.MaskingName AS ModelMaskingName
		,BV.HostURL
		,BV.OriginalImagePath
	FROM BikeVersions BV WITH (NOLOCK)
	INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes BM WITH (NOLOCK) ON BM.ID = BMO.BikeMakeId
	WHERE BV.ID = @VersionId
		AND BV.IsDeleted = 0
		AND BMO.IsDeleted = 0
		AND BM.IsDeleted = 0
		AND BV.Futuristic = 0
		AND BMO.Futuristic = 0
		AND BM.Futuristic = 0

	-- Query to get the pricing
	SELECT CI.ItemName
		,DSP.ItemValue AS Price
		,DSP.DealerId
		,DSP.ItemId
	FROM BW_NewBikeDealerShowroomPrices DSP WITH (NOLOCK)
	INNER JOIN BW_PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = DSP.ItemId AND DSP.DealerId = @DealerId
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DSP.DealerId
	WHERE DSP.CityId = @CityId
		AND DSP.BikeVersionId = @VersionId
		AND D.IsDealerDeleted = 0
		AND D.IsDealerActive = 1
		AND D.ApplicationId = 2
		AND DSP.ItemValue > 0
		AND (
			@DealerId IS NOT NULL
			AND DSP.DealerId = @DealerId
			)
	ORDER BY DSP.ItemId
	
	-- SELECT CLAUSE TO GET DEALER INFO
	SELECT 
		D.ID
		,D.FirstName
		,D.LastName
		,D.Address1 AS Address
		,campaign.Number AS PhoneNo
		,D.MobileNo
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
		,(
			CASE 
				WHEN P.Id IN (84,87) THEN 'Standard'
				WHEN P.Id IN (83,86) THEN 'Deluxe'
				WHEN P.Id IN (82,85) THEN 'Premium'
			END
		) AS DealerPackageType
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

	--Get Booking Amount of a bike added by Ashwini
	SELECT BA.Amount AS Amount FROM BW_DealerBikeBookingAmounts BA WITH(NOLOCK) WHERE BA.VersionId = @VersionId AND BA.DealerId = @DealerId AND BA.IsActive = 1

	--Get Num Of Days
	SELECT ISNULL(BAV.NumOfDays,0) AS NumOfDays FROM BW_BikeAvailability BAV WITH (NOLOCK) WHERE BAV.BikeVersionId = @VersionId AND BAV.DealerId = @DealerId AND BAV.IsActive = 1

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
	
	--Get EMI details offered by dealer
	SELECT LA.Id
		,ISNULL(LA.LTV, 0) LTV
		,ISNULL(LA.Tenure, 0) Tenure
		,ISNULL(LA.RateOfInterest, 0) RateOfInterest
		,LA.LoanProvider
		,ISNULL(MinDownPayment, 0) AS MinDownPayment
        ,ISNULL(MaxDownPayment, 0) AS MaxDownPayment
		,ISNULL(MinTenure, 0) AS MinTenure
        ,ISNULL(MaxTenure, 0) AS MaxTenure
        ,ISNULL(MinRateOfInterest, 0) AS MinRateOfInterest
        ,ISNULL(MaxRateOfInterest, 0) AS MaxRateOfInterest
        ,ISNULL(ProcessingFee, 0) AS ProcessingFee
	FROM BW_DealerLoanAmounts LA WITH (NOLOCK)
	WHERE LA.DealerId = @DealerId AND LA.IsActive = 1

	-- SELECT CLAUSE TO GET OFFER LIST OFFERED BY DEALER FOR GIVEN MODEL
	SELECT PQO.OfferCategoryId
		,PQO.OfferText
		,ISNULL(PQO.OfferValue, 0) OfferValue
		,PQO.EntryDate
		,PQOC.OfferType
		,PQO.OfferId -- Added by Sangram on 5 oct 2015
		,Case when BO.id is NULL then 'False' else 'True'end as 'IsOfferTerms'-- Added by Sangram on 5 oct 2015
		,PQO.IsPriceImpact
		,PQO.DealerId
		,PQO.OfferValidTill
	FROM BW_PQ_Offers AS PQO WITH (NOLOCK)
	INNER JOIN BW_PQ_OfferCategories AS PQOC WITH (NOLOCK) ON PQO.OfferCategoryId = PQOC.Id
	INNER JOIN BikeModels AS BMO WITH (NOLOCK) ON BMO.ID = PQO.ModelId
	LEFT OUTER JOIN BW_offers as BO WITH (NOLOCK) ON  PQO.offerid = BO.id -- Added by Sangram on 5 oct 2015
		AND BMO.IsDeleted = 0
		AND BMO.New = 1
	WHERE PQO.IsActive = 1
		AND PQOC.IsActive = 1
		AND PQO.DealerId = @DealerId
		AND PQO.CityId = @CityId
		AND PQO.OfferValidTill > GETDATE()
		AND PQO.ModelId = @ModelId
	ORDER BY PQOC.Id

	--Get benefits offered by dealer
	SELECT 
		benefits.Id AS 'BenefitId'
		,benefits.DealerID AS 'DealerId'
		,benefits.CatId AS 'CatId'
		,benefitCat.[Name] AS 'CategoryText'
		,benefits.[BenefitText] AS 'BenefitText'
		,city.Name AS 'City'
	FROM BW_PQ_DealerBenefit benefits WITH (NOLOCK)
	INNER JOIN [dbo].[BW_PQ_DealerBenefit_Category] benefitCat WITH (NOLOCK)
	ON benefits.CatId = benefitCat.Id AND benefitCat.IsActive = 1 
	AND benefits.IsActive = 1 AND benefits.DealerId = @DealerId AND benefits.CityId = @CityId
	INNER JOIN Cities city WITH(NOLOCK)
	ON benefits.CityId = city.Id AND city.ID = @CityId

	-- Query to get the dealer disclaimer
	SELECT Disclaimer
	FROM BW_DealerDisclaimer WITH (NOLOCK)
	WHERE DealerId = @DealerId
		AND BikeVersionId = @VersionId
		AND IsActive = 1

	--Get Secondary Dealer
	;WITH cte
	AS(
	SELECT DISTINCT
		d.ID
		,campaign.DealerName AS Organization
		,A.NAME AS AreaName
		,campaign.Number AS MaskingNumber
		,(
			CASE 
				WHEN P.Id IN (84,87) THEN 3
				WHEN P.Id IN (83,86) THEN 2
				WHEN P.Id IN (82,85) THEN 1
			END
		) AS DealerPackageType
	FROM Dealers AS D WITH (NOLOCK)	
	INNER JOIN Areas AS A WITH (NOLOCK) ON A.ID = D.AreaId AND (@DealerId IS NULL OR D.ID <> @DealerId)
	INNER JOIN BikeVersions BV WITH(NOLOCK) ON BV.BikeModelId = @ModelId AND BV.New = 1 AND BV.IsDeleted = 0
	INNER JOIN BW_NewBikeDealerShowroomPrices ndsp WITH(NOLOCK) ON ndsp.BikeVersionId = BV.ID AND ndsp.CityId = @CityId 
	AND ndsp.DealerId = D.ID AND ISNULL(ndsp.ItemValue,0) > 0
	INNER JOIN BW_PQ_DealerCampaigns campaign WITH(NOLOCK) ON d.IsDealerActive = 1 and d.IsDealerDeleted = 0 and d.ApplicationId = 2 AND d.Id = campaign.DealerId AND campaign.IsActive = 1
	INNER JOIN TC_ContractCampaignMapping contractCampaign WITH(NOLOCK) ON campaign.ContractId = contractCampaign.ContractId AND contractCampaign.ContractStatus = 1 AND campaign.ID = contractCampaign.CampaignId
	INNER JOIN BW_PQ_CampaignRules rules WITH(NOLOCK) ON campaign.Id = rules.CampaignId AND rules.IsActive = 1 
	AND rules.ModelId = @modelId AND rules.CityId = @cityId	
	INNER JOIN ContractStatus CS WITH(NOLOCK) ON CS.Id = contractCampaign.ContractStatus 
	INNER JOIN RVN_DealerPackageFeatures RDP WITH(NOLOCK) ON RDP.DealerPackageFeatureID = contractCampaign.ContractId
	INNER JOIN Packages P WITH(NOLOCK) ON P.Id = RDP.PackageId
	WHERE 
		d.IsDealerActive = 1
		AND d.IsDealerDeleted = 0
		AND d.ApplicationId = 2
	)
	SELECT * FROM cte 
	ORDER BY DealerPackageType,NEWID()  -- To order the list by randomly
END
