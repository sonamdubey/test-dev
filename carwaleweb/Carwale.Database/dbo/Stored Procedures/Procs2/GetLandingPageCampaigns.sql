IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLandingPageCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLandingPageCampaigns]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 03/08/2016
-- EXEC [GetLandingPageCampaigns]
-- =============================================
CREATE PROCEDURE [dbo].[GetLandingPageCampaigns]
AS
BEGIN
	SELECT LPC.Id
		,LPC.[Name]
		,LPC.[Type]
		,LPC.CreatedOn
		,OU.UserName CreatedBy
		,LPC.UpdatedOn
		,OU2.UserName UpdatedBy
		,LPLD.[Type] LeadDestinationType
		,LPLD.PQCampaignId
		,LPLD.DealerId
	FROM LandingPageCampaign LPC WITH (NOLOCK)
	INNER JOIN OprUsers OU WITH (NOLOCK) ON LPC.CreatedBy = OU.Id
		AND LPC.IsActive = 1
	INNER JOIN OprUsers OU2 WITH (NOLOCK) ON LPC.UpdatedBy = OU2.Id
	INNER JOIN LandingPageLeadDestination LPLD WITH (NOLOCK) ON LPC.Id = LPLD.CampaignId
END

