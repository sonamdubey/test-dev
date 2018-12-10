IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePQCampaignTemplate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePQCampaignTemplate]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 31st Oct 2014
-- Description:	To save Templates for campaigns(this SP is called from InsertorUpdatePQDealers)
-- =============================================
CREATE PROCEDURE [dbo].[SavePQCampaignTemplate] 
	@CampaignId INT,
	@TemplateId	VARCHAR(20)
AS
BEGIN
	SELECT CampaignId FROM PQ_DealerAd_Template_Platform_Maping WHERE CampaignId = @CampaignId
	IF @@ROWCOUNT > 0
	BEGIN
		DELETE FROM PQ_DealerAd_Template_Platform_Maping WHERE CampaignId = @CampaignId
	END

	INSERT INTO PQ_DealerAd_Template_Platform_Maping(CampaignId,AssignedTemplateId,PlatformId)
	SELECT @CampaignId,ListMember,AT.PlatformId 
	FROM fnSplitCSV(@TemplateId) FV 
	INNER JOIN PQ_SponsoredDealeAd_Templates AT on AT.TemplateId = FV.ListMember
END
