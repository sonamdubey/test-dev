IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LogLandingPageCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LogLandingPageCampaign]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 04/08/2016
-- Description:	Log changes done in Landing page campaign
-- =============================================
CREATE PROCEDURE [dbo].[LogLandingPageCampaign]
	@CampaignId INT
	,@Changes VARCHAR(MAX)
	,@LogMessage VARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO LandingPageCampaign_Log (
		CampaignId
		,Type
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,Changes
		,LogMessage
		)
	SELECT Id
		,Type
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,@Changes
		,@LogMessage
	FROM LandingPageCampaign WITH (NOLOCK)
	WHERE id = @CampaignId
END

