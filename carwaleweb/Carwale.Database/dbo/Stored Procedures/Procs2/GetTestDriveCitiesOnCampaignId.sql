IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTestDriveCitiesOnCampaignId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTestDriveCitiesOnCampaignId]
GO

	
-- =============================================
-- Author:		Chetan Thambad
-- Create date: 03-08-2016
-- Description:	Get Test drive cities on campaign Id
-- =============================================
CREATE PROCEDURE [dbo].[GetTestDriveCitiesOnCampaignId] @CampaignId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT LP.StateId
		,LP.CityId
	FROM LandingPageCampaign LC WITH (NOLOCK)
	INNER JOIN LandingPageCities LP WITH (NOLOCK) ON LC.Id = LP.CampaignId
	WHERE LC.Id = @CampaignId
END
