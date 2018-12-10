IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveTemplateMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveTemplateMapping]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 17/10/2016
-- Description:	To save template mappings to a campaign
-- =============================================
CREATE PROCEDURE [dbo].[SaveTemplateMapping]
	@CampaignId INT,
	@AssignedTemplateId INT = NULL,
	@PlatformId INT,
	@AssignedGroupId INT = NULL
AS
BEGIN
	INSERT into PQ_DealerAd_Template_Platform_Maping (CampaignId,AssignedTemplateId,PlatformId,AssignedGroupId)
	VALUES(@CampaignId,@AssignedTemplateId,@PlatformId,@AssignedGroupId)
END

