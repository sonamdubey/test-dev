IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCampaignDetailsById]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCampaignDetailsById]
GO

	-- ===================================================================================================
-- Created by: Vinayak, 20/07/2016
-- Desc: Get campaign details by campaignId
-- Exec GetCampaignDetailsById 4649
-- ===================================================================================================

CREATE PROCEDURE [dbo].[GetCampaignDetailsById] @CampaignId INT
AS

BEGIN
	SELECT DISTINCT vGAC.StartDate
		,vGAC.EndDate
		,vGAC.TotalGoal AS LeadTarget
		,vGAC.TotalCount LeadTargetAchieved
		,vGAC.DailyGoal AS DailyLeadTarget
		,vGAC.DailyCount AS DailyLeadTargetAchieved
		,DS.IsThirdPartyCampaign AS IsThirdPartyCampaign
		,DS.isFeaturedEnabled AS IsFeaturedEnabled
		,vGAC.ContractBehaviour AS ContractBehaviour
	FROM PQ_DealerSponsored DS WITH (NOLOCK)
	JOIN vwRunningCampaigns vGAC WITH (NOLOCK) ON vGAC.CampaignId = DS.Id 
	WHERE DS.Id = @CampaignId
END

