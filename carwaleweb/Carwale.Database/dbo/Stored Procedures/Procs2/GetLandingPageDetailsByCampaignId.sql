IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLandingPageDetailsByCampaignId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLandingPageDetailsByCampaignId]
GO

	

-- =============================================
-- Author:		Vinayak
-- Create date: 08-08-2016
-- Description:	Get Test drive details on campaign Id
-- =============================================
CREATE PROCEDURE [dbo].[GetLandingPageDetailsByCampaignId] @CampaignId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT LC.Name
	,LC.Type
	,LC.PrimaryHeading
	,LC.SecondaryHeading 
	,LC.IsEmailRequired
	,LC.DefaultModel
	,LC.ButtonText
	,LC.TrailingText
	,LC.IsDesktop
	,LC.IsMobile
	,(select TOP 1 Template from PQ_SponsoredDealeAd_Templates P WITH (NOLOCK) where P.TemplateId=LC.DesktopTemplateId) as DesktopHtml
	,(select TOP 1 Template from PQ_SponsoredDealeAd_Templates P WITH (NOLOCK) where P.TemplateId=LC.MobileTemplateId) as MobileHtml
	,LD.Type AS CampaignType
	,LD.PQCampaignId
	,LD.DealerId
	FROM LandingPageCampaign LC WITH (NOLOCK)
	INNER JOIN LandingPageLeadDestination LD WITH (NOLOCK) ON LC.Id = LD.CampaignId
	WHERE LC.Id = @CampaignId

	SELECT LM.MakeId
	,LM.ModelId
	FROM LandingPageCampaign LC WITH (NOLOCK)
	INNER JOIN LandingPageModels LM WITH (NOLOCK) ON LC.Id = LM.CampaignId
	WHERE LC.Id = @CampaignId

	SELECT LP.StateId
	,LP.CityId
	FROM LandingPageCampaign LC WITH (NOLOCK)
	INNER JOIN LandingPageCities LP WITH (NOLOCK) ON LC.Id = LP.CampaignId
	WHERE LC.Id = @CampaignId
	
END

