IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealersLatLong_28042016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealersLatLong_28042016]
GO

	-- Author:		Ashish Kamble
-- Create date: 12 Jan 2015
-- Description:	To get Number of Dealers with their lattitude and longitude for the given version whose prices for the given version are entered.
-- Modified By : Ashwini Todkar on 21 Jan 2015 
-- Retrieved LeadServingDistance from dealers table
-- EXEC BW_GetDealersLatLong 760
-- Modified By	:	Sumit Kate on 21 Mar 2016
-- Description	:	Get the Dealers and their lat-long by subscription model
-- Modified By	:	Vivek Gupta on 12-04-2016, fetching dealer's commte distance by joing with table DealerAreaCommuteDIstance
-- Modified By	:	Sumit Kate on 15 Apr 2016
-- Description	:	refer ContractStatus instead of IsActiveContract
-- Modified By	:	Sumit Kate on 28 Apr 2016
-- Description	:	return one more result set which indicates whether secondary dealers are available/not
-- EXEC BW_GetDealersLatLong_28042016 112,2
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealersLatLong_28042016] 
	@VersionId INT
	,@AreaId INT	
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CityId INT,@modelId INT	
	DECLARE @DealerId INT

	DECLARE @IsDealerAvailable BIT = 0

	SELECT @CityId = CityId
	FROM Areas WITH(NOLOCK)
	WHERE ID = @AreaId
	
	SELECT @modelId = BV.BikeModelId 
	FROM BikeVersions BV WITH(NOLOCK)
	WHERE BV.New = 1 AND BV.IsDeleted = 0 AND BV.ID = @versionId

	SELECT TOP 1 @DealerId = d.ID 
		FROM Dealers d WITH(NOLOCK)
		INNER JOIN BW_PQ_DealerCampaigns campaign WITH(NOLOCK) ON d.IsDealerActive = 1 and d.IsDealerDeleted = 0 and d.ApplicationId = 2 AND campaign.IsActive = 1 AND d.Id = campaign.DealerId AND campaign.IsActive = 1
		INNER JOIN TC_ContractCampaignMapping contractCampaign WITH(NOLOCK) ON campaign.ContractId = contractCampaign.ContractId AND contractCampaign.ContractStatus = 1 AND campaign.Id = contractCampaign.CampaignId
		INNER JOIN BW_PQ_CampaignRules rules WITH(NOLOCK) ON campaign.Id = rules.CampaignId AND rules.IsActive = 1 		
			AND rules.ModelId = @modelId AND rules.CityId = @cityId
		INNER JOIN DealerAreaCommuteDIstance DAC WITH(NOLOCK) ON d.id=dac.Dealerid and dac.IsActive = 1
		INNER JOIN BW_NewBikeDealerShowroomPrices nbsp WITH(NOLOCK) ON nbsp.BikeVersionId = @VersionId AND nbsp.CityId = @CityId AND nbsp.DealerId = d.ID
		AND ISNULL(nbsp.ItemValue,0) > 0
		WHERE (ISNULL(campaign.DealerLeadServingRadius,0) = 0
				OR
			   	ISNULL(campaign.DealerLeadServingRadius,0)	>= DAC.Distance
				)
				AND DAC.AreaId = @AreaId
		ORDER BY
			DAC.Distance	

	SELECT 
		@IsDealerAvailable =
		CASE 
			when COUNT(d.ID) > 0 
			THEN 1 
			ELSE 0 
		END
	FROM Dealers AS D WITH (NOLOCK)		
	INNER JOIN BW_NewBikeDealerShowroomPrices ndsp WITH(NOLOCK) ON ndsp.BikeVersionId = @VersionId AND ndsp.CityId = @CityId 
	AND ndsp.DealerId = D.ID AND ISNULL(ndsp.ItemValue,0) > 0	
	INNER JOIN BW_PQ_DealerCampaigns campaign WITH(NOLOCK) ON d.IsDealerActive = 1 and d.IsDealerDeleted = 0 and d.ApplicationId = 2 AND d.Id = campaign.DealerId AND campaign.IsActive = 1
	INNER JOIN TC_ContractCampaignMapping contractCampaign WITH(NOLOCK) ON campaign.ContractId = contractCampaign.ContractId AND contractCampaign.ContractStatus = 1 AND campaign.ID = contractCampaign.CampaignId
	INNER JOIN BW_PQ_CampaignRules rules WITH(NOLOCK) ON campaign.Id = rules.CampaignId AND rules.IsActive = 1 
	AND rules.ModelId = @modelId AND rules.CityId = @cityId	
	INNER JOIN BW_NewBikeDealerShowroomPrices nbsp WITH(NOLOCK) ON nbsp.BikeVersionId = @VersionId AND nbsp.CityId = @CityId AND nbsp.DealerId = d.ID
		AND ISNULL(nbsp.ItemValue,0) > 0
	WHERE 
		d.IsDealerActive = 1
		AND d.IsDealerDeleted = 0
		AND d.ApplicationId = 2
		AND (@DealerId IS NULL OR D.ID <> @DealerId)

	 SELECT @DealerId AS DealerId, @IsDealerAvailable AS IsDealerAvailable
END

