IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTemplateGroups]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTemplateGroups]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 14/10/2016
-- Description:	Fetch the template groups
-- =============================================
CREATE PROCEDURE [dbo].[GetTemplateGroups] 
	@PlatformId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Id, Name FROM TemplateGroup TG WITH(NOLOCK) 
	WHERE PlatformId = @PlatformId
END

