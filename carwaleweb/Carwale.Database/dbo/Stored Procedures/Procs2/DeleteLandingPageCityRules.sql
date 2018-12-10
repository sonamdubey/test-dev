IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteLandingPageCityRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteLandingPageCityRules]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 03/08/2016
-- Description:	To delete city rules for Landing page campaign
-- =============================================
CREATE PROCEDURE [dbo].[DeleteLandingPageCityRules] @CampaignId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE LandingPageCities
	WHERE CampaignId = @CampaignId
END

