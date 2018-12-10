IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteLandingPageModelRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteLandingPageModelRules]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 03/08/2016
-- Description:	To delete model rules for Landing page campaign
-- =============================================
CREATE PROCEDURE [dbo].[DeleteLandingPageModelRules] @CampaignId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE LandingPageModels
	WHERE CampaignId = @CampaignId
END

