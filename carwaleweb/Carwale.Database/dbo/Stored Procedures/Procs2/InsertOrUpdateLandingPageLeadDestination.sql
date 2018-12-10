IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOrUpdateLandingPageLeadDestination]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOrUpdateLandingPageLeadDestination]
GO
	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/07/2016
-- Description:	Add or update lead destination for landing page campaign
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrUpdateLandingPageLeadDestination] @CampaignId INT
	,@Type INT
	,@PQCampaignId INT = NULL
	,@DealerId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CampaignId
	FROM LandingPageLeadDestination WITH (NOLOCK)
	WHERE CampaignId = @CampaignId

	IF @@ROWCOUNT > 0
	BEGIN
		UPDATE LandingPageLeadDestination
		SET Type = @Type
			,PQCampaignId = @PQCampaignId
			,DealerId = @DealerId
		WHERE CampaignId = @CampaignId
	END
	ELSE
	BEGIN
		INSERT INTO LandingPageLeadDestination (
			CampaignId
			,Type
			,PQCampaignId
			,DealerId
			)
		VALUES (
			@CampaignId
			,@Type
			,@PQCampaignId
			,@DealerId
			)
	END
END

