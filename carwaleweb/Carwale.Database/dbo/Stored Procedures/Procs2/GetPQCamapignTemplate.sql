IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQCamapignTemplate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQCamapignTemplate]
GO

	


-- =============================================      
-- Author:  <Vinayak>      
-- Create date: <4/5/16>      
-- Description: <get html template for pq sponsored ads>  
-- exec [GetPQCamapignTemplate] 4545,74
--==============================================
CREATE PROCEDURE [dbo].[GetPQCamapignTemplate] @PlatformId INT
	,@CampaignId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from      
	-- interfering with SELECT statements.      
	SET NOCOUNT ON;

	SELECT P.TemplateId
	FROM pq_dealerad_template_platform_maping M WITH (NOLOCK)
	INNER JOIN pq_sponsoreddealead_templates P WITH (NOLOCK) ON M.AssignedTemplateId = P.TemplateId
	WHERE M.CampaignId = @CampaignId
		AND M.PlatformId = @PlatformId
END

