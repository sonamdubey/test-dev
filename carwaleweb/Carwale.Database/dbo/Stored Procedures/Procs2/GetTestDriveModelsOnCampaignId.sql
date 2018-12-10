IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTestDriveModelsOnCampaignId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTestDriveModelsOnCampaignId]
GO

	
-- =============================================
-- Author:		Chetan Thambad
-- Create date: 03-08-2016
-- Description:	Get Test drive Make Models on campaign Id
-- =============================================
CREATE PROCEDURE [dbo].[GetTestDriveModelsOnCampaignId] @CampaignId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT LM.MakeId
		,LM.ModelId
	FROM LandingPageCampaign LC WITH (NOLOCK)
	INNER JOIN LandingPageModels LM WITH (NOLOCK) ON LC.Id = LM.CampaignId
	WHERE LC.Id = @CampaignId
END
