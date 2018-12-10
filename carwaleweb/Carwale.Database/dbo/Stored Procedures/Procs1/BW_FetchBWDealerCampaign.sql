IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_FetchBWDealerCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_FetchBWDealerCampaign]
GO

	 
--=============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Show BW Dealer Campaign
-- Modified By	:	Sumit Kate on 15 Apr 2016
-- Description	:	refer ContractStatus instead of IsActiveContract
-- Parameters	:
--	@CampaignId	:	BW Campaign Id
-- EXEC	[BW_FetchBWDealerCampaign] 1
-- =============================================
CREATE PROCEDURE [dbo].[BW_FetchBWDealerCampaign] @CampaignId INT
AS
BEGIN
	SELECT campaign.Id AS CampaignId
		,campaign.DealerName
		,campaign.DealerEmailId
		,campaign.Number
		,campaign.IsActive
		,campaign.IsBookingAvailable
		,campaign.DealerLeadServingRadius
		,d.MobileNo AS DealerMobile
	FROM BW_PQ_DealerCampaigns campaign WITH (NOLOCK)
	INNER JOIN TC_ContractCampaignMapping mapping WITH (NOLOCK) ON campaign.ContractId = mapping.ContractId
		AND campaign.Id = mapping.CampaignId
		AND mapping.ContractStatus = 1
		AND mapping.ApplicationId = 2
	INNER JOIN Dealers d WITH(NOLOCK) ON campaign.DealerId = d.id
	WHERE campaign.Id = @CampaignId
END
