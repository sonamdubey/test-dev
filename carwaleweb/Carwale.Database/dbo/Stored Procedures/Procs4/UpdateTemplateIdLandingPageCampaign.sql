IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateTemplateIdLandingPageCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateTemplateIdLandingPageCampaign]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 05/08/2016
-- Description:	To update the templateId in landingpage campaign
-- =============================================
CREATE PROCEDURE [dbo].[UpdateTemplateIdLandingPageCampaign] @CampaignId INT
	,@TemplateId INT
	,@PlatformId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE LandingPageCampaign
	SET DesktopTemplateId = @TemplateId
	WHERE Id = @CampaignId
		AND @PlatformId = 1 -- For desktop

	UPDATE LandingPageCampaign
	SET MobileTemplateId = @TemplateId
	WHERE Id = @CampaignId
		AND @PlatformId = 43 -- For desktop
END

