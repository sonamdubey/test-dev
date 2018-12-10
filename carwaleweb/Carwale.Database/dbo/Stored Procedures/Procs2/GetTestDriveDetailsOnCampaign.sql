IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTestDriveDetailsOnCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTestDriveDetailsOnCampaign]
GO

	
-- =============================================
-- Author	:	Vinayak
-- Description	:	Get test drive details on campaign 
-- Execute [dbo].[GetTestDriveDetailsOnCampaign] 1
-- ============================================
CREATE PROCEDURE [dbo].[GetTestDriveDetailsOnCampaign] @CampaignId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT LC.NAME AS DealerName
		,LC.IsEmailRequired
		,LC.ButtonText
		,LC.TrailingText
	FROM LandingPageCampaign LC WITH (NOLOCK)
	INNER JOIN LandingPageCities LP WITH (NOLOCK) ON LC.Id = LP.CampaignId
	INNER JOIN LandingPageModels LM WITH (NOLOCK) ON LC.Id = LM.CampaignId
	WHERE LC.Id = @CampaignId
END

