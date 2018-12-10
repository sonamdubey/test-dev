IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetDealerPQAdTemplates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetDealerPQAdTemplates]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	: 23/11/15
-- Description:	get list of template id
-- Modified By Jitendra added extra column TemplateType
-- =============================================
CREATE PROCEDURE [dbo].[CW_GetDealerPQAdTemplates]
	@PlatformId SMALLINT,
	@TypeId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TemplateId,TemplateName,Template,TemplateType
	FROM PQ_SponsoredDealeAd_Templates WITH(NOLOCK)
	WHERE PlatformId = @PlatformId AND CategoryId=@TypeId
END

