IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteTemplateMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteTemplateMapping]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 17/10/2016
-- Description:	Delete the template mappings of a campaign
-- =============================================
CREATE PROCEDURE [dbo].[DeleteTemplateMapping]
	@CampaignId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CampaignId FROM PQ_DealerAd_Template_Platform_Maping WITH(NOLOCK) WHERE CampaignId = @CampaignId
	IF @@ROWCOUNT > 0
	BEGIN
		DELETE FROM PQ_DealerAd_Template_Platform_Maping WHERE CampaignId = @CampaignId
	END
END

