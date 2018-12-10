IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLandingPageCampaignDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLandingPageCampaignDetails]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 04/08/2016
-- EXEC [GetLandingPageCampaignDetails] 7
-- =============================================
CREATE PROCEDURE [dbo].[GetLandingPageCampaignDetails] @CampaignId INT
AS
BEGIN
	SELECT LPC.Id
		,LPC.[Name]
		,LPC.[Type]
		,LPC.PrimaryHeading
		,LPC.SecondaryHeading
		,LPC.IsEmailRequired
		,LPC.DefaultModel
		,LPC.ButtonText
		,LPC.TrailingText
		,LPLD.[Type] LeadDestinationType
		,LPLD.PQCampaignId
		,LPLD.DealerId
		,LPC.IsDesktop
		,LPC.IsMobile
		,ISNULL(LPC.DesktopTemplateId, - 1)
		,ISNULL(LPC.MobileTemplateId, - 1)
		,PSDAT.Template DesktopHtml
		,PSDAT2.Template MobileHtml
	FROM LandingPageCampaign LPC WITH (NOLOCK)
	INNER JOIN LandingPageLeadDestination LPLD WITH (NOLOCK) ON LPC.Id = LPLD.CampaignId
		AND LPC.Id = @CampaignId
	LEFT OUTER JOIN PQ_SponsoredDealeAd_Templates PSDAT WITH (NOLOCK) ON LPC.DesktopTemplateId = PSDAT.TemplateId
	LEFT OUTER JOIN PQ_SponsoredDealeAd_Templates PSDAT2 WITH (NOLOCK) ON LPC.MobileTemplateId = PSDAT2.TemplateId

	SELECT LPM.Id
		,LPM.CampaignId
		,LPM.MakeId
		,LPM.ModelId
	FROM LandingPageModels LPM WITH (NOLOCK)
	WHERE LPM.CampaignId = @CampaignId

	SELECT LPC.Id
		,LPC.CampaignId
		,LPC.StateId
		,LPC.CityId
	FROM LandingPageCities LPC WITH (NOLOCK)
	WHERE LPC.CampaignId = @CampaignId
END

