IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllTemplateGroups]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllTemplateGroups]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <05/10/2016>
-- Description:	<Fetch templates groups for specified template type and platform type>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllTemplateGroups]
	@TemplateType INT,
	@PlatformType INT
AS
BEGIN
	SET NOCOUNT ON
	SELECT TGM.Groupid AS Id, TG.Name, SDT.TemplateName, SDT.Template AS Template
	FROM TemplateGroup TG WITH (NOLOCK)
	LEFT JOIN TemplateGroupMapping TGM WITH (NOLOCK) ON TG.Id = TGM.GroupId AND TG.IsActive = 1
	LEFT JOIN PQ_SponsoredDealeAd_Templates SDT WITH (NOLOCK) ON TGM.TemplateId = SDT.TemplateId 
	WHERE SDT.TemplateType = @TemplateType AND SDT.PlatformId = @PlatformType
END

