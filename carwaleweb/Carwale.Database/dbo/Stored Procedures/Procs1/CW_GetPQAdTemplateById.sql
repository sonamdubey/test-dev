IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetPQAdTemplateById]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetPQAdTemplateById]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	: 23/11/15
-- Description : Get pqad template by Id
-- Modified By Jitendra added extra column TemplateType
-- =============================================
CREATE PROCEDURE [dbo].[CW_GetPQAdTemplateById]
	@TemplateId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TemplateId,TemplateName,Template,TemplateType
	FROM PQ_SponsoredDealeAd_Templates WITH(NOLOCK)
	WHERE TemplateId = @TemplateId
END

