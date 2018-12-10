IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_FetchBWCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_FetchBWCampaigns]
GO

	--=============================================
-- Created on	:	Sumit Kate on 18 Mar 2016
-- Description	:	Show BW Dealer Campaigns
-- Modified By	:	Sumit Kate on 15 Apr 2016
-- Description	:	refer ContractStatus instead of IsActiveContract
-- Parameters	:
--	@ContractId	:	BW Contract Id
-- EXEC	[BW_FetchBWCampaigns] 10922
-- =============================================
CREATE PROCEDURE [dbo].[BW_FetchBWCampaigns] @ContractId INT
AS
BEGIN
	SELECT campaign.Id AS CampaignId
		,campaign.DealerName
		,campaign.DealerEmailId
		,campaign.Number
		,campaign.IsActive
		,CONVERT(BIT, CASE 
				WHEN mapping.CampaignId = campaign.Id
					THEN 1
				ELSE 0
				END) AS IsMapped
		,campaign.IsBookingAvailable
		,campaign.DealerLeadServingRadius
		,dealer.Organization
		,dealer.Id AS DealerId
		,dealer.MobileNo -- added by Sangram on 15/04/2016 
	FROM  TC_ContractCampaignMapping mapping WITH (NOLOCK)
	LEFT JOIN BW_PQ_DealerCampaigns campaign WITH (NOLOCK) ON mapping.ContractId = campaign.ContractId 		
		AND mapping.ContractStatus = 1
		AND mapping.ApplicationId = 2
	LEFT JOIN Dealers dealer WITH (NOLOCK) ON dealer.Id = mapping.DealerId
	WHERE mapping.ContractId = @ContractId
END
