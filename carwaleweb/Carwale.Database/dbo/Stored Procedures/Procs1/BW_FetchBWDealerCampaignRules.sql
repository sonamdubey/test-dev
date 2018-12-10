IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_FetchBWDealerCampaignRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_FetchBWDealerCampaignRules]
GO

	

--=============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Show BW Dealer Campaign
-- Modified By	:	Sumit Kate on 15 Apr 2016
-- Description	:	refer ContractStatus instead of IsActiveContract
-- Parameters	:
--	@CampaignId	:	BW Campaign Id
--	@DealerID	:	Dealer Id
-- =============================================
CREATE PROCEDURE [dbo].[BW_FetchBWDealerCampaignRules]
	@CampaignId INT,
	@DealerID INT
AS
BEGIN
	SELECT 
		campaignRules.Id,
		BM.Name AS MakeName,
		BMO.Name AS ModelName,
		city.Name AS CityName,
		states.Name As StateName
	FROM 
	BW_PQ_CampaignRules campaignRules WITH(NOLOCK)
	INNER JOIN BW_PQ_DealerCampaigns campaign WITH(NOLOCK) 
	ON campaignRules.CampaignId = campaign.ID AND campaign.DealerId = @DealerID
	INNER JOIN TC_ContractCampaignMapping mapping WITH(NOLOCK)
	ON campaign.ContractId = mapping.ContractId AND campaign.Id = mapping.CampaignId
	AND mapping.DealerId = @DealerID AND mapping.ContractStatus = 1 AND mapping.ApplicationId = 2
	INNER JOIN Cities city WITH(NOLOCK)
	ON campaignRules.CityId = city.ID AND city.IsDeleted = 0
	INNER JOIN States states WITH(NOLOCK)
	ON city.StateId = states.ID AND states.IsDeleted = 0
	INNER JOIN BikeModels BMO WITH(NOLOCK)
	ON campaignRules.ModelId = BMO.ID AND BMO.IsDeleted = 0 AND BMO.New = 1
	INNER JOIN BikeMakes BM WITH(NOLOCK)
	ON BMO.BikeMakeId = BM.ID AND BMO.IsDeleted = 0 AND BM.New = 1
	WHERE campaignRules.CampaignId = @CampaignId AND campaignRules.IsActive = 1
	ORDER BY campaignRules.EnteredBy DESC
END
