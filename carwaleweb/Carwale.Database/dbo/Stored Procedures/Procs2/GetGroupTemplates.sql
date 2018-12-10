IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetGroupTemplates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetGroupTemplates]
GO

	
-- =============================================
-- Author:		Sanjay Soni
-- Create date: 05/10/2016 
-- Description:	get all group templates
-- exec [dbo].[GetGroupTemplates] 1,1
-- =============================================
CREATE PROCEDURE [dbo].[GetGroupTemplates]
	@GroupId INT,
    @PlatformId INT
AS
BEGIN
	SET NOCOUNT ON;

	IF (@GroupId != -1)
	BEGIN
		SELECT TGM.Groupid AS Id
			,AT.TemplateId
			,AT.Template
			,TG.Name
			,(CASE 
				WHEN TGM.Groupid = @GroupId
					THEN 1
				ELSE 0
				END) AS IsBelongToGroup
		FROM PQ_SponsoredDealeAd_Templates AT WITH(NOLOCK)
			LEFT JOIN TemplateGroupMapping TGM  WITH(NOLOCK) ON TGM.TemplateId = AT.TemplateId AND TGM.GroupId = @GroupId
			LEFT JOIN TemplateGroup TG WITH(NOLOCK) ON TG.Id = TGM.GroupId
		WHERE AT.PlatformId = @PlatformId AND AT.CategoryId = 1
		ORDER BY IsBelongToGroup DESC
	END
	ELSE
	BEGIN
	SELECT TemplateId, Template
		FROM PQ_SponsoredDealeAd_Templates WITH(NOLOCK)
		WHERE PlatformId = @PlatformId AND CategoryId = 1
	END

END

